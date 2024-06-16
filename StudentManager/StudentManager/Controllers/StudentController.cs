using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManager;
using StudentManager.Models.Dtos;
using StudentManager.Services;
using StudentManager.Services.Interfaces;

namespace StudentManager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<GetStudentsDto>> GetStudents(
            int page = 1, int pageSize = 10,
            string search = "", string sort = "asc", string sortBy = "FirstName"
            ) 
        {
            try
            {
                var res = await _studentService.GetStudentsAsync(page, pageSize, search, sort, sortBy);
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(string id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);

                if (student == null)
                {
                    return NotFound();
                }

                return Ok(student);   
            } catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(string id, Student student)
        {
            try
            {
                var res =await _studentService.UpdateStudentAsync(id, student);
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // POST: api/Student
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(StudentDto student)
        {
            Student studentEntity;
            try
            {
                studentEntity = await _studentService.CreateStudentAsync(student);
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Error creating student");
                throw;
            }

            return CreatedAtAction("GetStudent", new { id = studentEntity.Id }, student);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            try
            {
                await _studentService.DeleteStudentAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return NoContent();
        }
    }
}
