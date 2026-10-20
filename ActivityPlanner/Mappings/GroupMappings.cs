using ActivityPlanner.DTOs;
using ActivityPlanner.Extensions;
using ActivityPlanner.Models;
using System.Runtime.CompilerServices;

namespace ActivityPlanner.Mappings
{
    public static class GroupMappings
    {
        public static GroupDetailsDto ToDetailsDto(this Group g, int currentUserId) => new()
        {
            GroupId = g.GroupId,
            Name = g.Name,
            Description = g.Description,
            Budget = g.Budget,
            InvitationCode = g.IsAdmin(currentUserId) ? g.InvitationCode : null,
            Members = g.Members.Select(m => new GroupMemberDto
            {
                UserId = m.UserId,
                Username = m.User.Username,
                GroupId = m.GroupId,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                MembershipStatus = m.Status
            }).ToList()
        };

        public static Group DtoToEntity(this CreateGroupDto dto, int currentUserId) => new()
        {

            Name = dto.Name,
            Description = dto.Description,
            Budget = dto.Budget,
            InvitationCode = Guid.NewGuid().ToString("N")[..8].ToUpper(),
            Members =
            {
                new GroupMember
                {
                    UserId =  currentUserId,
                    Role = GroupRole.Admin,
                    Status = MembershipStatus.Approved
                }
            }
            
        };

        public static void ApplyTo(this UpdateGroupDto dto, Group group)
        {
            group.Name = dto.Name;
            group.Description = dto.Description;
            group.Budget = dto.Budget;

        }

        public static GroupMember ToGroupMember(this Group group, int userId) => new()
        {
            UserId = userId,
            GroupId = group.GroupId,
            Role = GroupRole.Member,
            JoinedAt = DateTime.UtcNow,
            Status = MembershipStatus.Approved
        };



    }
}
