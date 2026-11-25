using ActivityPlanner.Models;

namespace ActivityPlanner.Extensions
{
    public static class LocationExtensions
    {
        public static string ToDisplayName(this Location l) => l.ToString().Replace('_', ' ');
    }
}
