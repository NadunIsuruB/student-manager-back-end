using StudentManager.Models.Dtos;

namespace StudentManager.Services.Interfaces;

public interface IStudentService
{
    Task<Student> CreateStudentAsync(StudentDto student);
    Task<Student> GetStudentByIdAsync(string id);
    Task<GetStudentsDto> GetStudentsAsync(int page = 1, int pageSize = 10, string search = "", string sort = "asc", string sortBy = "FirstName");
    Task<Student> UpdateStudentAsync(string id, Student student);
    Task DeleteStudentAsync(string id);
}