using System.Globalization;

namespace ActivityPlanner.Models
{
    public class PlannedActivity
    {
        public int Id {  get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Location { get; set; } = null!;
        public List<Interests> Interests {  get; set;} = new List<Interests>();


    }
}
