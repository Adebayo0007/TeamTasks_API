using TeamTask_API.Domain.Entities;
namespace TeamTask_API.Domain.Services;
public interface ITeamService
{
    Task<Guid> CreateTeamAsync(Guid creatorUserId, string name);
    Task AddUserToTeamAsync(Guid teamId, Guid adminUserId, Guid userIdToAdd);
}