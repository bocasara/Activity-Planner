using ActivityPlanner.Data;
using ActivityPlanner.DTOs;
using ActivityPlanner.Mappings;
using ActivityPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityPlanner.Repositories
{
    public class ActivityRepository
    {
        private readonly ActivityPlannerContext _context;

        public ActivityRepository(ActivityPlannerContext context)
        {
            _context = context;
        }

        public async Task<List<Activity>> GetActivities()
        {
            return await _context.Activities.ToListAsync();
        }

        public async Task<Activity?> GetActivityById(int activityId)
        {
            return await _context.Activities.FindAsync(activityId);
        }

        public async Task<Activity> CreateActivity(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task UpdateActivity(Activity activity, UpdateActivityDto dto)
        {
            dto.ApplyTo(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteActivity(Activity activity)
        {
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}
