using TeamTask_API.Domain.Entities;
namespace TeamTask_API.Domain.Repositories;
public interface ITeamUserRepository
{
    Task<TeamUser?> GetAsync(Guid userId, Guid teamId);
}