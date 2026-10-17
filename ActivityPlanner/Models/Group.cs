namespace ActivityPlanner.Models
{
    public class Group
    {
        public int GroupId {  get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Budget { get; set; }
        public string InvitationCode { get; set; } = null!;
        public List<GroupMember> Members { get; set; } = new();
    }
}
