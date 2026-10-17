namespace ActivityPlanner.DTOs
{
    public class UpdateGroupDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = "";
        public decimal Budget { get; set; }

    }
}
