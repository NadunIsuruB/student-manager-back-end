using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManager;
using StudentManager.Models.Dtos;

namespace StudentManager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public UserController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return await _context.Users.Select(u => new UserDto
            {
                Name = u.Name,
                Email = u.Email
            }).ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        // POST: api/User/Login
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(UserLogin user)
        {
            var password = Util.HashPassword(user.Password);
            var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == password);
            if (userDb == null)
            {
                return NotFound();
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

            return Ok("Bearer " + token);
        }

        // POST: api/User
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CreateUserResponseDto>> PostUser(CreateUserDto user)
        {
            // Hash password
            var userEntity = new User();
            userEntity.Name = user.Name;
            userEntity.Email = user.Email;
            userEntity.Password = Util.HashPassword(user.Password);
            
            _context.Users.Add(userEntity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(userEntity.Id))
                {
                    return Conflict("Something went wrong, try again!");
                }
                else if (UserExists(userEntity.Email))
                {
                    return Conflict("User exits with Email");
                }
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = userEntity.Id }, new CreateUserResponseDto()
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                Name = userEntity.Name,
                Password = user.Password
            });
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
