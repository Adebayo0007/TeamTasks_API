using Microsoft.EntityFrameworkCore;
using TeamTask_API.Domain.Entities;

namespace TeamTask_API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamUser> TeamUsers { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).IsRequired().HasMaxLength(200);
            e.Property(u => u.FullName).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Team>(e =>
        {
            e.Property(t => t.Name).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<TeamUser>(e =>
        {
            e.HasKey(tu => new { tu.TeamId, tu.UserId });
            e.HasOne(tu => tu.Team).WithMany(t => t.TeamUsers).HasForeignKey(tu => tu.TeamId);
            e.HasOne(tu => tu.User).WithMany(u => u.TeamUsers).HasForeignKey(tu => tu.UserId);
        });

        modelBuilder.Entity<TaskItem>(e =>
        {
            e.HasOne(t => t.Team).WithMany(t => t.Tasks).HasForeignKey(t => t.TeamId);
            e.HasOne(t => t.CreatedByUser).WithMany(u => u.CreatedTasks).HasForeignKey(t => t.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(t => t.AssignedToUser).WithMany(u => u.AssignedTasks).HasForeignKey(t => t.AssignedToUserId).OnDelete(DeleteBehavior.SetNull);
            e.Property(t => t.Title).IsRequired().HasMaxLength(200);
        });
    }
}
