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

namespace StudentManager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Student
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents(
            int page = 1, int pageSize = 10,
            string search = "", string sort = "asc", string sortBy = "FirstName"
            ) 
        {
            // add pagination, sorting and searching to get
            IQueryable<Student> students = _context.Students;

            if (!string.IsNullOrEmpty(search))
            {
                students = students.Where(s => 
                        s.FirstName.Contains(search) || 
                        s.LastName.Contains(search) ||
                        s.Email.Contains(search) ||
                        s.Address.Contains(search) ||
                        s.Mobile.Contains(search) ||
                        s.DateOfBirth.ToString().Contains(search) ||
                        s.NIC.Contains(search));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                students = sort.ToLower() == "asc" ? 
                    students.OrderBy(Util.GetSortingPropertySelectorLambda(sortBy)) : 
                    students.OrderByDescending(Util.GetSortingPropertySelectorLambda(sortBy));
            }
                
            var filteredStudents = await students
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return Ok(filteredStudents);
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(string id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Student/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(string id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Student
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(StudentDto student)
        {
            Student studentEntity = new Student();
            studentEntity.Id = Guid.NewGuid().ToString();
            studentEntity.FirstName = student.FirstName;
            studentEntity.LastName = student.LastName;
            studentEntity.Mobile = student.Mobile;
            studentEntity.Email = student.Email;
            studentEntity.NIC = student.NIC;
            studentEntity.DateOfBirth = student.DateOfBirth;
            studentEntity.Address = student.Address;
            
            _context.Students.Add(studentEntity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(studentEntity.Id))
                {
                    return Conflict();
                }
                else if(StudentExists(studentEntity.Email))
                {
                    return Conflict();
                }
                else if (StudentExists(studentEntity.NIC))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent", new { id = studentEntity.Id }, student);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
