using Microsoft.Build.Framework;

namespace StudentManager.Models.Dtos;

public class UserDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
}