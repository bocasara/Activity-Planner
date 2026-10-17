using ActivityPlanner.Data;
using ActivityPlanner.Models;
namespace ActivityPlanner.Repositories
{
    public class ActivityRepository
    {
        private readonly ActivityPlannerContext _context;

        public ActivityRepository(ActivityPlannerContext context)
        {
            _context = context;
        }

        public async Task<List<PlannedActivity>> GetActivities() {
             return _context.Activities.ToList();
        }

        public async Task<PlannedActivity?> GetActivityById(int activityId)
        {
            var activity = await _context.Activities.FindAsync(activityId);
            return activity;
        }

        public async Task CreateActivity(PlannedActivity newActivity)
        {
            await _context.Activities.AddAsync(newActivity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateActivity(int actvityId, PlannedActivity updatedActivity)
        {
            var activity = await _context.Activities.FindAsync(actvityId);
            activity.Name = updatedActivity.Name;
            activity.Description = updatedActivity.Description;
            activity.Price = updatedActivity.Price;
            activity.Interests = updatedActivity.Interests;
            activity.Location = updatedActivity.Location;
            await _context.SaveChangesAsync();

        }

        public async Task RemoveActivity(int actvityId)
        {
            var activity = await _context.Activities.FindAsync(actvityId);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}


