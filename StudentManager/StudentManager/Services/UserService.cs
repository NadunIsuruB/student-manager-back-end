using StudentManager.Models.Dtos;
using StudentManager.Services.Interfaces;

namespace StudentManager.Services;

public class UserService: IUserService
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<User> GetUser(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetUsers()
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateUser(string id, User user)
    {
        throw new NotImplementedException();
    }

    public Task<CreateUserResponseDto> CreateUser(CreateUserDto user)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> LoginUser(UserLogin user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUser(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UserExists(string id)
    {
        throw new NotImplementedException();
    }
}