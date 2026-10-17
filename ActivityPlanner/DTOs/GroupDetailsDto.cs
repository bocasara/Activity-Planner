using ActivityPlanner.Models;
using System.Text.Json.Serialization;

namespace ActivityPlanner.DTOs
{
    public class GroupDetailsDto
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Budget { get; set; }
        public List<GroupMemberDto> Members { get; set; } = new();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InvitationCode { get; set; }
    }
}
