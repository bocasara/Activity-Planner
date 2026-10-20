using System.ComponentModel.DataAnnotations;

namespace ActivityPlanner.DTOs
{
    public class AddMemberDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; } = null!;
    }
}
