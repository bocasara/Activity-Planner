using ActivityPlanner.DTOs;
using ActivityPlanner.Models;
using System.Runtime.CompilerServices;

namespace ActivityPlanner.Mappings
{
    public static class ActivityMappings
    {
        public static Activity ToEntity(this CreateActivityDto dto) => new()
        {

            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Location = dto.Location.Value,
            Interests = dto.Interests

        };

        public static void ApplyTo(this UpdateActivityDto dto, Activity activity)
        {
            activity.Name = dto.Name;
            activity.Description = dto.Description;
            activity.Price = dto.Price;
            activity.Location = dto.Location.Value;
            activity.Interests = dto.Interests;
        }

        public static RecommendedActivityDto ToRecommendedDto(this Activity a, int matchedMembers, int totalMembers, int overlapCount,List<Interests> sharedInterests) => new()
        {
            ActivityId = a.Id,
            Name = a.Name,
            Description = a.Description,
            Price = a.Price,
            Location = a.Location,
            Interests = a.Interests.ToList(),
            MatchedMembers = matchedMembers,
            TotalMembers = totalMembers,
            OverlapCount = overlapCount,
            SharedInterests = sharedInterests,
            ScorePercent = totalMembers == 0 ? 0 : (int)Math.Round(100.0 * matchedMembers / totalMembers)
        };
    }
}
