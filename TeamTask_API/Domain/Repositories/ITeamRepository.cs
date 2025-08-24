using TeamTask_API.Domain.Entities;
namespace TeamTask_API.Domain.Repositories;
public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id);
    Task AddAsync(Team team);
    Task<bool> IsUserInTeamAsync(Guid userId, Guid teamId);
    Task AddUserToTeamAsync(TeamUser teamUser);
    Task<bool> SaveChangesAsync();
}