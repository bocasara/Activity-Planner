using ActivityPlanner.Models;

namespace ActivityPlanner.DTOs
{
    public class RecommendedActivityDto
    {
        public int ActivityId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public Location Location { get; set; }
        public List<Interests> Interests { get; set; } = [];
        public int ScorePercent { get; set; }
        public int MatchedMembers { get; set; }
        public int TotalMembers { get; set; }
        public int OverlapCount { get; set; }
        public List<Interests> SharedInterests { get; set; } = [];
    }

}
