using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wyklad5.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Services
{
    public interface IStudentDbService
    {
        public IActionResult EnrollStudent(EnrollStudentRequest enrollStudentRequest);
        public IActionResult PromoteStudents(PromotionStudentRequest promotionStudentRequest);
    }
}
