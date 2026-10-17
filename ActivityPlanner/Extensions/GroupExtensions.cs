using ActivityPlanner.Models;

namespace ActivityPlanner.Extensions
{
    public static class GroupExtensions
    {
        public static bool IsApprovedMember(this Group group, int userId) =>
            group.Members.Any(m => m.UserId == userId && m.Status == MembershipStatus.Approved);

        public static bool IsAdmin(this Group group, int userId) =>
            group.Members.Any(m => m.UserId == userId
                                && m.Role == GroupRole.Admin
                                && m.Status == MembershipStatus.Approved);

        public static bool IsLastAdmin(this Group group, int userId) =>
            group.IsAdmin(userId) &&
            group.Members.Count(m => m.Role == GroupRole.Admin && m.Status == MembershipStatus.Approved) == 1;
    }
}