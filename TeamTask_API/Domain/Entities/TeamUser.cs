using TeamTask_API.Domain.Enums;

namespace TeamTask_API.Domain.Entities;

public class TeamUser
{
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public TeamRole Role { get; set; } = TeamRole.Member;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
