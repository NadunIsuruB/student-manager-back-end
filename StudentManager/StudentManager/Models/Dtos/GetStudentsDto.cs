namespace StudentManager.Models.Dtos;

public class GetStudentsDto
{
    public int page { get; set; }
    public int pageSize { get; set; }
    public int total { get; set; }
    public IEnumerable<Student> students { get; set; }
}