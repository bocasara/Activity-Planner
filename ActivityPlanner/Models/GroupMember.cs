namespace ActivityPlanner.Models
{

    public class GroupMember
    {
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public GroupRole Role { get; set; } = GroupRole.Member;
        public MembershipStatus Status { get; set; } = MembershipStatus.Approved;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    }
}
