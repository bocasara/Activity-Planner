using ActivityPlanner.Data;
using ActivityPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityPlanner.Repositories
{
    public class UserRepository
    {
        private readonly ActivityPlannerContext _context;
        public UserRepository(ActivityPlannerContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExists(string email, int? exceptCurrentUser = null)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.UserId != exceptCurrentUser);
        }

        public async Task<bool> UsernameExists(string username, int? exceptCurrentUser = null)
        {
            return await _context.Users.AnyAsync(u => u.Username == username && u.UserId != exceptCurrentUser);
        }

        public async Task<bool> IsLastAdminInGroup(int userId)
        {
            return await _context.Groups.AnyAsync(g => 
                g.Members.Count > 1 && 
                g.Members.Any(
                    m => m.UserId == userId &&
                    m.Role == GroupRole.Admin) &&
                g.Members.Count(m => m.Role == GroupRole.Admin) == 1);
        }

        public async Task<User> CreateAccount(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAccount(User user) 
        {
            var soloGroups = await _context.Groups.
                Where(g => g.Members.Count == 1 && g.Members.Any(m => m.UserId == user.UserId)).ToListAsync();

            _context.Groups.RemoveRange(soloGroups);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccount(User user)
        {
            await _context.SaveChangesAsync();
        }
    }
}
