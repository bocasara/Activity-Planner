using ActivityPlanner.DTOs;
using ActivityPlanner.Models;
using System.Runtime.CompilerServices;

namespace ActivityPlanner.Mappings
{
    public static class ActivityMappings
    {
        public static PlannedActivity ToEntity(this CreateActivityDto dto) => new()
        {

            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Location = dto.Location,
            Interests = dto.Interests

        };

        public static void ApplyTo(this UpdateActivityDto dto, PlannedActivity activity)
        {
            activity.Name = dto.Name;
            activity.Description = dto.Description;
            activity.Price = dto.Price;
            activity.Location = dto.Location;
            activity.Interests = dto.Interests;
        }
    }
}
