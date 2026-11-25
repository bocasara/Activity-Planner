using ActivityPlanner.Models;
using System.ComponentModel.DataAnnotations;

namespace ActivityPlanner.DTOs
{
    public class GroupPreferenceDto
    {
        [Required]
        [Range(0, 100000)]
        public decimal? Budget { get; set; }

        [Required]
        public Location? Location { get; set; }
    }
}
