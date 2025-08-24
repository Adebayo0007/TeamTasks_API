using TeamTask_API.Domain.Entities;
namespace TeamTask_API.Domain.Repositories;
public interface ITaskRepository
{
    Task<List<TaskItem>> GetTasksByTeamAsync(Guid teamId);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task AddAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
    Task<bool> SaveChangesAsync();
}