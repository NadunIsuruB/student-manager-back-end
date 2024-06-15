namespace StudentManager.Models.Dtos;

public class CreateUserResponseDto : UserDto
{
    public string Id { get; set; }
    public string Password { get; set; }
}