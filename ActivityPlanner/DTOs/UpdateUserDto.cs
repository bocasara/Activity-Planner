using ActivityPlanner.Models;
using System.ComponentModel.DataAnnotations;

namespace ActivityPlanner.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = null!;
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;
        public List<Interests> Interests { get; set; } = new List<Interests>();
    }
}
