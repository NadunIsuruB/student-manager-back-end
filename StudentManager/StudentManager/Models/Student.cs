namespace StudentManager;

public class Student : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string NIC { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
}