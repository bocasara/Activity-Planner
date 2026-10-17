using ActivityPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace ActivityPlanner.Data
{
    public class ActivityPlannerContext(DbContextOptions<ActivityPlannerContext> options) : DbContext(options)
    {



        public DbSet<PlannedActivity> Activities { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

          
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GroupMember>(e =>{
                e.HasKey(gm => new { gm.GroupId, gm.UserId });

                e.HasOne(gm => gm.Group)
                    .WithMany(g => g.Members)
                    .HasForeignKey(gm => gm.GroupId);

                e.HasOne(gm => gm.User)
                    .WithMany(u => u.MemberShips)
                    .HasForeignKey(gm => gm.UserId);
            });

            modelBuilder.Entity<Group>()
                .Property(g => g.Budget)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PlannedActivity>()
                .Property(a => a.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Group>()
                .Property(g => g.InvitationCode)
                .HasMaxLength(50);

            modelBuilder.Entity<Group>()
                .HasIndex(g => g.InvitationCode)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(256);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<GroupMember>()
                .Property(gm => gm.Role)
                .HasConversion<string>();

            modelBuilder.Entity<GroupMember>()
                .Property(gm => gm.Status)
                .HasConversion<string>();

            // EF compares values to decide whether a propery changed, and for a list it only compares the reference so it won't see if i add a new interest
            // A value comparer tells ef how to compare and copy the list


            // SequenceEquals checks if lists a & b are equal, a!, b! tells the compiler the lists are not null
            // a - snapshot (previous list), b - current list 
            var interestsComparer = new ValueComparer<List<Interests>>(
                (a, b) => a!.SequenceEqual(b!),
                v => v.Aggregate(0, (hash, i) => HashCode.Combine(hash, i)), // Walks through the list and combines the 
                v => v.ToList()); // this makes a snapshot of the interests list to remember how it is loaded so it can make a changes

            modelBuilder.Entity<User>()
                .Property(u => u.Interests)
                .HasConversion(
                    v => string.Join(',', v.Select(e => e.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(e => Enum.Parse<Interests>(e)).ToList()
                ).Metadata.SetValueComparer(interestsComparer);

            modelBuilder.Entity<PlannedActivity>()
                .Property(a => a.Interests)
                .HasConversion(
                    v => string.Join(',', v.Select(e => e.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(e => Enum.Parse<Interests>(e)).ToList()
                 )
                .Metadata.SetValueComparer(interestsComparer);

            modelBuilder.Entity<User>().HasData(
              new User { UserId = 1, Username = "ana", Email = "ana@test.com", PasswordHash = "placeholder", Interests = [Interests.Food, Interests.Movies] },
              new User { UserId = 2, Username = "mihai", Email = "mihai@test.com", PasswordHash = "placeholder", Interests = [Interests.Sports, Interests.Games] },
              new User { UserId = 3, Username = "dana", Email = "dana@test.com", PasswordHash = "placeholder", Interests = [Interests.Outdoors, Interests.Food] });

            modelBuilder.Entity<Group>().HasData(
                new Group { GroupId = 1, Name = "Friday crew", Description = "", Budget = 20, InvitationCode = "FRIDAY1" });

            modelBuilder.Entity<GroupMember>().HasData(
                new GroupMember { GroupId = 1, UserId = 1, Role = GroupRole.Admin, Status = MembershipStatus.Approved, JoinedAt = new DateTime(2026, 10, 1) },
                new GroupMember { GroupId = 1, UserId = 2, Role = GroupRole.Member, Status = MembershipStatus.Approved, JoinedAt = new DateTime(2026, 10, 1) },
                new GroupMember { GroupId = 1, UserId = 3, Role = GroupRole.Member, Status = MembershipStatus.Approved, JoinedAt = new DateTime(2026, 10, 1) });

        }

    }
}
