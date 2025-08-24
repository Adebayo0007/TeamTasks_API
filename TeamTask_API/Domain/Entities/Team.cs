using TeamTask_API.Domain.Enums;

namespace TeamTask_API.Domain.Entities;

public class Team
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
