using ActivityPlanner.DTOs;
using ActivityPlanner.Mappings;
using ActivityPlanner.Models;

namespace ActivityPlanner.Services
{
    public class RecommendationService
    {
        public List<RecommendedActivityDto> Recommend(IEnumerable<User> members, IEnumerable<Activity> activities, decimal budget, Location location)
        {
            var counted = members.Where(m => m.Interests.Count > 0).ToList();

            return activities
                .Where(a => a.Price <= budget && (a.Location == location || a.Location == Location.Anywhere))
                .Select(a => Score(a, counted))
                .OrderByDescending(r => r.MatchedMembers)
                .ThenByDescending(r => r.OverlapCount)
                .ThenBy(r => r.Price)
                .ThenBy(r => r.Name)
                .ToList();
        }
        private static RecommendedActivityDto Score(Activity activity, List<User> counted)
        {
            var matchedMembers = 0;
            var overlap = 0;

            foreach (var member in counted)
            {
                var shared = member.Interests.Intersect(activity.Interests).Count();
                if (shared > 0) matchedMembers++;
                overlap += shared;
            }

            var sharedInterests = activity.Interests
                .Where(i => counted.Any(m => m.Interests.Contains(i)))
                .Distinct()
                .ToList();

            return activity.ToRecommendedDto(matchedMembers, counted.Count, overlap, sharedInterests);
        }

    }

    
}
