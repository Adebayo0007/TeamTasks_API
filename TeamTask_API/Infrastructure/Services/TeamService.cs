using TeamTask_API.Domain.Entities;
using TeamTask_API.Domain.Repositories;
using TeamTask_API.Domain.Services;
using TeamTask_API.Domain.Enums;

namespace TeamTask_API.Infrastructure.Services;
public class TeamService : ITeamService
{
    private readonly ITeamRepository _teams;
    private readonly IUserRepository _users;

    public TeamService(ITeamRepository teams, IUserRepository users)
    {
        _teams = teams; _users = users;
    }

    public async Task<Guid> CreateTeamAsync(Guid creatorUserId, string name)
    {
        var team = new Team { Name = name };
        await _teams.AddAsync(team);
        await _teams.AddUserToTeamAsync(new TeamUser { TeamId = team.Id, UserId = creatorUserId, Role = TeamRole.Admin });
        await _teams.SaveChangesAsync();
        return team.Id;
    }

    public async Task AddUserToTeamAsync(Guid teamId, Guid adminUserId, Guid userIdToAdd)
    {
        var team = await _teams.GetByIdAsync(teamId) ?? throw new KeyNotFoundException("Team not found.");
        var adminMembership = team.TeamUsers.FirstOrDefault(tu => tu.UserId == adminUserId);
        if (adminMembership == null || adminMembership.Role != TeamRole.Admin)
            throw new UnauthorizedAccessException("Only team admins can add users.");
        if (team.TeamUsers.Any(tu => tu.UserId == userIdToAdd)) return;
        await _teams.AddUserToTeamAsync(new TeamUser { TeamId = teamId, UserId = userIdToAdd, Role = TeamRole.Member });
        await _teams.SaveChangesAsync();
    }
}
