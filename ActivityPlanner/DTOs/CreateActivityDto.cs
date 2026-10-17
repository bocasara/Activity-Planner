using ActivityPlanner.Models;

namespace ActivityPlanner.DTOs
{
    public class CreateActivityDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public string Location { get; set; } = null!;
        public List<Interests> Interests { get; set; } = new List<Interests>();
    }
}
