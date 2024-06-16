using StudentManager.Models.Dtos;

namespace StudentManager.Services.Interfaces;

public interface IUserService
{
    public Task<User> GetUser(string id);
    public Task<IEnumerable<User>> GetUsers();
    public Task<User> UpdateUser(string id, User user);
    public Task<CreateUserResponseDto> CreateUser(CreateUserDto user);
    public Task<UserDto> LoginUser(UserLogin user);
    public Task DeleteUser(string id);
    public Task<bool> UserExists(string id);
}