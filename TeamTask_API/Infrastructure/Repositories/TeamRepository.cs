using Microsoft.EntityFrameworkCore;
using TeamTask_API.Data;
using TeamTask_API.Domain.Entities;
using TeamTask_API.Domain.Repositories;

namespace TeamTask_API.Infrastructure.Repositories;
public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _db;
    public TeamRepository(AppDbContext db) => _db = db;

    public Task<Team?> GetByIdAsync(Guid id) =>
        _db.Teams.Include(t => t.TeamUsers).FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(Team team) => await _db.Teams.AddAsync(team);

    public async Task AddUserToTeamAsync(TeamUser teamUser)
    {
        await _db.TeamUsers.AddAsync(teamUser);
    }

    public Task<bool> IsUserInTeamAsync(Guid userId, Guid teamId) =>
        _db.TeamUsers.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == userId);

    public Task<bool> SaveChangesAsync() => _db.SaveChangesAsync().ContinueWith(t => t.Result > 0);
}
