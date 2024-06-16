using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudentManager.Models.Dtos;
using StudentManager.Services.Interfaces;

namespace StudentManager.Services;

public class StudentService: IStudentService
{
    private readonly ApplicationDbContext _context; 
    
    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetStudentsDto> GetStudentsAsync(int page = 1, int pageSize = 10, string search = "", string sort = "asc",
        string sortBy = "FirstName")
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
                s.CreatedAt.ToString().Contains(search) ||
                s.NIC.Contains(search));
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            students = sort.ToLower() == "asc" ? 
                students.OrderBy(Util.GetSortingPropertySelectorLambda(sortBy)) : 
                students.OrderByDescending(Util.GetSortingPropertySelectorLambda(sortBy));
        }

            
        var studentCount = await students.CountAsync();
            
        var response = new GetStudentsDto();
        response.students = await students
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        response.page = page;
        response.pageSize = pageSize;
        response.total = studentCount;
            
        return response;
    }
    
    public async Task<Student> CreateStudentAsync(StudentDto student)
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
        await _context.SaveChangesAsync();
        return studentEntity;
    }

    public async Task<Student> GetStudentByIdAsync(string id)
    {
        var student = await _context.Students.FindAsync(id);
        return student;
    }

    public async Task<Student> UpdateStudentAsync(string id, Student student)
    {
        if (id != student.Id)
        {
            throw new Exception("Id mismatch");
        }

        _context.Entry(student).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return student;
    }

    public async Task DeleteStudentAsync(string id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            throw new Exception("Student not found");
        }

        student.Delete();
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }
}