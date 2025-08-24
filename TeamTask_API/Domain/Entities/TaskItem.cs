using TeamTask_API.Domain.Enums;

namespace TeamTask_API.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Status Status { get; set; } = Status.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid TeamId { get; set; }
    public Team Team { get; set; } = default!;

    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = default!;

    public Guid? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }
}
