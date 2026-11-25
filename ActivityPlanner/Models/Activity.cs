using System.Globalization;

namespace ActivityPlanner.Models
{
    public class Activity
    {
        public int Id {  get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public Location Location { get; set; }
        public List<Interests> Interests {  get; set;} = [];


    }
}
