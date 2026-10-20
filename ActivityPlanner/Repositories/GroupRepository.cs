using ActivityPlanner.Data;
using ActivityPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityPlanner.Repositories
{
    public class GroupRepository
    {
        private readonly ActivityPlannerContext _context;
        public GroupRepository(ActivityPlannerContext context)
        {
            _context = context;
        }

        public async Task<Group?> GetGroupById(int groupId) {
            //Include says “also load the related GroupMember rows through the Members property" and then m is a groupmember entitity
            var group = await _context.Groups.Include(g => g.Members).ThenInclude(m => m.User).FirstOrDefaultAsync(g => g.GroupId == groupId);
            return group;
        }

        public async Task<Group> CreateGroup(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task DeleteGroup(Group group)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        public async Task<Group?> GetGroupByInvitationCode(string invitationCode)
        {
            var group = await _context.Groups.Include(g => g.Members).ThenInclude(m => m.User).FirstOrDefaultAsync(g => g.InvitationCode == invitationCode);
            return group;
        }

        public async Task<bool> RemoveMember(int userId, int groupId)
        {
            var groupMember = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
            if(groupMember == null) return false;

            _context.GroupMembers.Remove(groupMember);
            await _context.SaveChangesAsync();
            return true;
                
        }

        public async Task UpdateGroup(Group group)
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddMember(GroupMember groupMember)
        {
            _context.GroupMembers.Add(groupMember);
            await _context.SaveChangesAsync();
        }

    }
}
