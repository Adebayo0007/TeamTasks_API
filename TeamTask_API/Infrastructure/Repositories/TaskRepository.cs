using Microsoft.EntityFrameworkCore;
using TeamTask_API.Data;
using TeamTask_API.Domain.Entities;
using TeamTask_API.Domain.Repositories;

namespace TeamTask_API.Infrastructure.Repositories;
public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;
    public TaskRepository(AppDbContext db) => _db = db;

    public Task<List<TaskItem>> GetTasksByTeamAsync(Guid teamId) =>
        _db.Tasks.Where(t => t.TeamId == teamId).OrderByDescending(t => t.CreatedAt).ToListAsync();

    public Task<TaskItem?> GetByIdAsync(Guid id) =>
        _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(TaskItem task) => await _db.Tasks.AddAsync(task);

    public Task DeleteAsync(TaskItem task) { _db.Tasks.Remove(task); return Task.CompletedTask; }

    public Task<bool> SaveChangesAsync() => _db.SaveChangesAsync().ContinueWith(t => t.Result > 0);
}
