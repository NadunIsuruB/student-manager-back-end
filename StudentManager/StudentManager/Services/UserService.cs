using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManager.Models.Dtos;
using StudentManager.Services.Interfaces;

namespace StudentManager.Services;

public class UserService: IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    
    public UserService(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    
    public async Task<User> GetUser(string id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        return await _context.Users.Select(u => new UserDto
        {
            Name = u.Name,
            Email = u.Email
        }).ToListAsync();
    }

    public async Task<User> UpdateUser(string id, User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        user.Update();
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<CreateUserResponseDto> CreateUser(CreateUserDto user)
    {
        // Hash password
        var userEntity = new User();
        userEntity.Name = user.Name;
        userEntity.Email = user.Email;
        userEntity.Password = Util.HashPassword(user.Password);
            
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();
        return new CreateUserResponseDto()
        {
            Id = userEntity.Id,
            Email = userEntity.Email,
            Name = userEntity.Name,
            Password = user.Password
        };
    }

    public async Task<string> LoginUser(UserLogin user)
    {
        var password = Util.HashPassword(user.Password);
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == password);
        if (userDb == null)
        {
            throw new Exception("Invalid credentials");
        }
        // Generate JWT token
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            null,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);
        return "Bearer " + token;
    }

    public async Task DeleteUser(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("Not Found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public bool UserExists(string id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}