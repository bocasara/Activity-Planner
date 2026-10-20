using ActivityPlanner.Models;

namespace ActivityPlanner.DTOs
{
    public class UserDetailsDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<Interests> Interests { get; set; } = new();
    }
}
