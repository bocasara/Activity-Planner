using ActivityPlanner.DTOs;
using ActivityPlanner.Mappings;
using ActivityPlanner.Models;
using ActivityPlanner.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActivityPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UsersController(UserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        private int CurrentUserId => int.Parse(Request.Headers["X-User-Id"]!);

        [HttpGet("me")]
        public async Task<ActionResult<UserDetailsDto>> GetMe()
        {
            var user = await _userRepository.GetUserById(CurrentUserId);
            if (user == null) return NotFound();

            return Ok(user.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<UserDetailsDto>> CreateUser(CreateUserDto dto)
        {
            if (await _userRepository.EmailExists(dto.Email, CurrentUserId))
            {
                return Conflict("Email already exists.");
            }
            if(await _userRepository.UsernameExists(dto.Username, CurrentUserId))
            {
                return Conflict("Username already exists.");
            }

            var user = dto.ToEntity(_passwordHasher);
            var created = await _userRepository.CreateAccount(user);
            return CreatedAtAction(nameof(GetMe), new { userId = created.UserId }, created.ToDto());
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe(UpdateUserDto dto)
        {
            var user = await _userRepository.GetUserById(CurrentUserId);
            if (user == null) return NotFound();

            if (await _userRepository.EmailExists(dto.Email, CurrentUserId))
            {
                return Conflict("Email already exists.");
            }
            if(await _userRepository.UsernameExists(dto.Username, CurrentUserId))
            {
                return Conflict("Username already exists.");
            }

            dto.ApplyTo(user);
            await _userRepository.UpdateAccount(user);
            return NoContent();
        }

        [HttpDelete("me")]

        public async Task<IActionResult> DeleteMe()
        {
            var user = await _userRepository.GetUserById(CurrentUserId);
            if (user == null) return NotFound();

            if(await _userRepository.IsLastAdminInGroup(CurrentUserId))
            {
                return Conflict("Cannot delete account. You are the last admin in one or more groups.");
            }
            await _userRepository.DeleteAccount(user);
            return NoContent();
        }
    }
}
