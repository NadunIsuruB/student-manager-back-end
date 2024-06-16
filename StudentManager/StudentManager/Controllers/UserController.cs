using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManager.Models.Dtos;
using StudentManager.Services.Interfaces;

namespace StudentManager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var res = await _userService.GetUsers();
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            try
            {
                var user = await _userService.GetUser(id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            
            try
            {
                await _userService.UpdateUser(id, user);
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
            try
            {   
                var token = await _userService.LoginUser(user);
                return Ok(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // POST: api/User
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CreateUserResponseDto>> PostUser(CreateUserDto user)
        {
            try
            {
                var createUserResponseDto = await _userService.CreateUser(user);
                return Ok(createUserResponseDto);
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("User already exists");
                return Conflict();
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            _userService.DeleteUser(id);
            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _userService.UserExists(id);
        }
    }
}
