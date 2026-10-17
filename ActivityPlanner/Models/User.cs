namespace ActivityPlanner.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public List<Interests> Interests { get; set; } = new();
        public List<GroupMember> MemberShips { get; set; } = new ();


    }
}
