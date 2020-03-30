using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Cw3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IDbService _dbService;

        public StudentsController(IDbService service)
        {
            _dbService = service;
        }
        [HttpGet]
        public IActionResult GetStudents(string id)
        {
            var students = new List<Student>();
            
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s8652;Integrated Security=True"))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "Select FirstName, LastName, BirthDate, Name, Semester From Enrollment, Student, Studies Where Enrollment.IdStudy = Studies.IdStudy AND Enrollment.IdEnrollment = Student.IdEnrollment AND Student.IndexNumber = " + id+" ";

                    con.Open();
                    var dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        var st = new Student();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.BirthDate = dr["BirthDate"].ToString();
                        students.Add(st);
                    }
                }
            }
            return Ok(students);

        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            var enroll = new Enrollment();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s8652;Integrated Security=True"))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "select Semester from Student, Enrollment, Studies Where Student.IndexNumber=@IndexNumber AND Enrollment.IdStudy = Studies.IdStudy AND Enrollment.IdEnrollment = Student.IdEnrollment";
                    com.Parameters.AddWithValue("IndexNumber", id);
                    con.Open();
                    var dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        enroll.Semester = (int)dr["Semester"];
                    }

                }
            }
            return Ok(enroll);

        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student); 
        }
        [HttpDelete]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Deleted Completed");
        }
        [HttpPut]
        public IActionResult EditStudent(int id)
        {
            return Ok("Update completed");
        }
    }
}