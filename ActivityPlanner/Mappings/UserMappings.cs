using ActivityPlanner.DTOs;
using ActivityPlanner.Models;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace ActivityPlanner.Mappings
{
    public static class UserMappings
    {

        public static UserDetailsDto ToDto(this User user) => new()
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Interests = user.Interests
        };

        public static User ToEntity(this CreateUserDto dto, IPasswordHasher<User> passwordHasher)
        {
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Interests = dto.Interests
            };
            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);
            return user;
        }

       
        public static void ApplyTo(this UpdateUserDto dto, User user)
        {
            user.Username = dto.Username;
            user.Email = dto.Email;
            user.Interests = dto.Interests;
        }

    }
}
