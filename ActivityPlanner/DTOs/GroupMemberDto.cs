using ActivityPlanner.Models;

namespace ActivityPlanner.DTOs
{
    public class GroupMemberDto
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string Username { get; set; }
        public GroupRole Role { get; set; }
        public DateTime JoinedAt { get; set; }
        public MembershipStatus MembershipStatus { get; set; }

    }
}
