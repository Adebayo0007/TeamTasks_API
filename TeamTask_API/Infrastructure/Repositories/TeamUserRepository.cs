using Microsoft.EntityFrameworkCore;
using TeamTask_API.Data;
using TeamTask_API.Domain.Entities;
using TeamTask_API.Domain.Repositories;

namespace TeamTask_API.Infrastructure.Repositories;
public class TeamUserRepository : ITeamUserRepository
{
    private readonly AppDbContext _db;
    public TeamUserRepository(AppDbContext db) => _db = db;

    public Task<TeamUser?> GetAsync(Guid userId, Guid teamId) =>
        _db.TeamUsers.Include(tu => tu.Team).Include(tu => tu.User)
            .FirstOrDefaultAsync(tu => tu.TeamId == teamId && tu.UserId == userId);
}
