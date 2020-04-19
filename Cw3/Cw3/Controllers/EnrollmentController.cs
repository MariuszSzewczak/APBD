using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Wyklad5.DTOs.Requests;
using Cw3.Services;

namespace Cw3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest enrollment)
        {
            return _service.EnrollStudent(enrollment);
        }
        [HttpPost("/promote")]
        public IActionResult PromotionStudent(PromotionStudentRequest promotion)
        {

            return _service.PromoteStudents(promotion);
        }


    }
}