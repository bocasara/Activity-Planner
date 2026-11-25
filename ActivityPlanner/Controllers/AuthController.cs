using ActivityPlanner.DTOs;
using ActivityPlanner.Models;
using ActivityPlanner.Repositories;
using ActivityPlanner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActivityPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        UserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        TokenService tokenService) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            
            var user = await userRepository.GetUserByUsername(dto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = tokenService.CreateToken(user);
            return Ok(new AuthResponseDto
            {
                Token = token,
                UserId = user.UserId,
                Username = user.Username,
            });
        }
    }    
}
