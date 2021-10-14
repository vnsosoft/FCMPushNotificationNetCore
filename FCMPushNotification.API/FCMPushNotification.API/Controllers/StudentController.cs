using FCMPushNotification.Services.FirebaseModels;
using FirebaseDataServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCMPushNotification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _studentService.GetStudents();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Student student)
        {
            await _studentService.AddStudent(student);
            var response = await _studentService.GetStudents();
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(string id, Student student)
        {
            await _studentService.UpdateStudent(id, student);
            var response = await _studentService.GetStudents();
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _studentService.RemoveStudent(id);
            var response = await _studentService.GetStudents();
            return Ok(response);
        }
    }
}
