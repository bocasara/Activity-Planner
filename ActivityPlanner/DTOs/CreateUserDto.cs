using ActivityPlanner.Models;
using System.ComponentModel.DataAnnotations;

namespace ActivityPlanner.DTOs
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = null!;
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;
        public List<Interests> Interests { get; set; } = new List<Interests>();
    }
}
