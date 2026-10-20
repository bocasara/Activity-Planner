using ActivityPlanner.Models;
using System.ComponentModel.DataAnnotations;

namespace ActivityPlanner.DTOs
{
    public class CreateActivityDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Description { get; set; } = "";
        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }
        [Required]
        [StringLength(100)]
        public string Location { get; set; } = null!;
        [MinLength(1, ErrorMessage = "At least one interest must be selected.")]
        public List<Interests> Interests { get; set; } = new List<Interests>();
    }
}
