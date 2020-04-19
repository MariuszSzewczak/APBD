using Cw3.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            string path = httpContext.Request.Path;
            string queryString = httpContext.Request?.QueryString.ToString();
            string method = httpContext.Request.Method.ToString();
            string bodyParameters = String.Empty;
            if (httpContext.Request.Headers.ContainsKey("Index"))
            {
                string indexNumber = httpContext.Response.Headers["Index"].ToString();
                using (var con = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
                using (var com = new SqlCommand())
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    com.Transaction = tran;
                    com.Connection = con;
                    Student student = new Student();
                    com.Parameters.AddWithValue("IndexNumber", indexNumber);
                    com.CommandText = "Select count() as cnt from Student where IndexNumber = @IndexNumber)";
                    var dr = com.ExecuteReader();
                    dr.Read();
                    if ((int)dr["cnt"] == 1)
                    {
                        await httpContext.Response.WriteAsync("indexNumber Exist : @indexNumber");
                        return;
                    }
                    else
                    {
                        httpContext.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
                        await httpContext.Response.WriteAsync("indexNumber does not Exist");
                        return;
                    }
                }
            }

            using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true))
            {
                bodyParameters = await reader.ReadToEndAsync();
            }

            var LogWriter = new FileStream("requestLog.txt", FileMode.Create);
            using (var writer = new StreamWriter(LogWriter))
            {
                string text = $"Path: {path} \nQueryString:{queryString} \nMethod: {method} \nBody Parameters: {bodyParameters}";
                writer.WriteLine(text);
            }

            await _next(httpContext);
        }

    }
}
