using Microsoft.Build.Framework;

namespace StudentManager.Models.Dtos;

public class StudentDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Mobile { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string NIC { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
}