using ActivityPlanner.Models;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

namespace ActivityPlanner.DTOs
{
    public class GroupMemberDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public GroupRole Role { get; set; }
        public DateTime JoinedAt { get; set; }
        public MembershipStatus MembershipStatus { get; set; }

    }
}
