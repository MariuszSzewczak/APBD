using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using Wyklad5.DTOs.Requests;

namespace Cw3.Services
{
    public class SqlServerStudentDbService : ControllerBase, IStudentDbService
    {

        private int idEnrollment = 0;
        private int idStudy = 0;
        private DateTime startDate = DateTime.Now;

        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {

            using (var con = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                com.Transaction = tran;
                com.Connection = con;
                com.Parameters.AddWithValue("Name", request.Studies);
                com.CommandText = $"select idStudy from Studies where Name = @Name";

                var dr = com.ExecuteReader();

                while (dr.Read())
                {
                    idStudy = (int)dr["IdStudy"];
                }
                dr.Close();
                if (idStudy == 0)
                {
                    
                    return BadRequest("Studies : " + request.Studies + " does not exist!");
                }
                else
                {

                    com.CommandText = $"select max(startDate) from enrollment where semester = 1 and idStudy = {idStudy} ";
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                        com.CommandText = "Select max(idEnrollment) 'idEnrollment' from enrollment";
                        dr = com.ExecuteReader();
                        dr.Read();
                        idEnrollment = (int)dr["IdEnrollment"] + 1;
                        dr.Close();
                        com.Parameters.AddWithValue("idEnrollment", idEnrollment);
                        com.CommandText = "insert into enrollment (idEnrollment,Semester,idStudy,StartDate)" +
                             $"VALUES (@idEnrollment,1,{idStudy} " + ", '" + startDate + "')";
                        com.ExecuteNonQuery();
                        tran.Commit();


                    }
                    else
                    {
                        dr.Close();
                        tran.Rollback();
                    }

                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                    com.CommandText = "select count(*)'count' from Student Where indexNumber = @indexNumber";
                    dr = com.ExecuteReader();
                    dr.Read();
                    if ((int)dr["count"] > 0)
                    {
                        return BadRequest("IndexNumber : @indexNumber already exist!");
                    }
                    else
                    {
                        dr.Close();

                        com.Parameters.AddWithValue("FirstName", request.FirstName);
                        com.Parameters.AddWithValue("LastName", request.LastName);
                        com.Parameters.AddWithValue("BirthDate", request.Birthdate);
                        com.CommandText = "insert into Student VALUES (@indexNumber,@firstName,@LastName,@BirthDate,@idEnrollment)";
                        com.ExecuteNonQuery();

                    }
                }
            }
            return Ok("New Student has been enrolled : \n Name = " + request.FirstName + " \n Last Name = " + request.LastName);
        }

      
        public IActionResult PromoteStudents(PromotionStudentRequest promotionStudentRequest)
        {
            using (var con = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                com.Transaction = tran;
                com.Connection = con;
                com.Parameters.AddWithValue("Semester", promotionStudentRequest.Semester);
                com.Parameters.AddWithValue("idStudy", promotionStudentRequest.Studies);
                com.CommandText = "select count() 'cnt' from Enrollment where semester = @Semester " +
                                                    "and idStudy = (select idstudy from Studies where idstudy = @idStudy )";
                var dr = com.ExecuteReader();
                dr.Read();
                if ((int)dr["cnt"] > 0)
                {
                    dr.Close();
                    com.CommandText = "P_PromoteStudent";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    return NotFound("Not Found");
                }

            }
            return Ok();
        }
		 
		 public bool CheckCredential(string user, string pass)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                try
                {
                    command.CommandText = "Select 1 from student where IndexNumber = @user and pass = @pass; ";
                    command.Parameters.AddWithValue("user", user);
                    command.Parameters.AddWithValue("pass", pass);
                    return command.ExecuteReader().Read();

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

        }
    }
}
