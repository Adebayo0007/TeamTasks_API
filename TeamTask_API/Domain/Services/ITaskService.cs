using TeamTask_API.DTOs.Tasks;
namespace TeamTask_API.Domain.Services;
public interface ITaskService
{
    Task<List<TaskResponse>> GetTeamTasksAsync(Guid teamId, Guid userId);
    Task<TaskResponse> CreateTaskAsync(Guid teamId, Guid userId, CreateTaskRequest request);
    Task<TaskResponse> UpdateTaskAsync(Guid taskId, Guid userId, UpdateTaskRequest request);
    Task DeleteTaskAsync(Guid taskId, Guid userId);
    Task<TaskResponse> PatchStatusAsync(Guid taskId, Guid userId, DTOs.Tasks.StatusPatchRequest request);
}