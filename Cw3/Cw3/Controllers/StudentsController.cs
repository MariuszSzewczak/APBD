using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetStudents(string orderBy) {

            return Ok(_dbService.GetStudents());
            
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            }
            else if (id == 2)
            { return Ok("Szewczak"); }

            return NotFound("Nie ma studenta");
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
            if (id == 1)
            {
                return Ok("Delete Kowalski");
            }
            else if (id == 2)
            { return Ok("Delete Szewczak"); }

            return NotFound("Nie ma studenta");
        }
        [HttpPut]
        public IActionResult EditStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Edit Kowalski");
            }
            else if (id == 2)
            { return Ok("Edit Szewczak"); }

            return NotFound("Nie ma studenta");
        }
    }
}