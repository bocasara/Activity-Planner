using ActivityPlanner.Models;

namespace ActivityPlanner.Extensions
{
    public static class GroupExtensions
    {
        public static bool IsAdmin(this Group group, int userId)
        {
            var me = group.Members.FirstOrDefault(m => m.UserId == userId);
            return me.Role == GroupRole.Admin;
        }

    }
}
