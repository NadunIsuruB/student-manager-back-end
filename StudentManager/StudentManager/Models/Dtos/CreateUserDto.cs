using Microsoft.Build.Framework;

namespace StudentManager.Models.Dtos;

public class CreateUserDto : UserDto
{
    [Required]
    public string Password { get; set; }
}