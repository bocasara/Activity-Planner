using System.ComponentModel.DataAnnotations;

namespace ActivityPlanner.DTOs
{
    public class CreateGroupDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = "";
        [Range(0, 100000)]
        public decimal Budget { get; set; }
    }
}
