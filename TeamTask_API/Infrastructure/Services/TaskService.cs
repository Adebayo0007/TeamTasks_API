using AutoMapper;
using TeamTask_API.Domain.Entities;
using TeamTask_API.Domain.Repositories;
using TeamTask_API.Domain.Services;
using TeamTask_API.DTOs.Tasks;

namespace TeamTask_API.Infrastructure.Services;
public class TaskService : ITaskService
{
    private readonly ITaskRepository _tasks;
    private readonly ITeamRepository _teams;
    private readonly IUserRepository _users;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository tasks, ITeamRepository teams, IUserRepository users, IMapper mapper)
    {
        _tasks = tasks; _teams = teams; _users = users; _mapper = mapper;
    }

    public async Task<List<TaskResponse>> GetTeamTasksAsync(Guid teamId, Guid userId)
    {
        if (!await _teams.IsUserInTeamAsync(userId, teamId)) throw new UnauthorizedAccessException("Not a member of the team.");
        var items = await _tasks.GetTasksByTeamAsync(teamId);
        return items.Select(_mapper.Map<TaskResponse>).ToList();
    }

    public async Task<TaskResponse> CreateTaskAsync(Guid teamId, Guid userId, CreateTaskRequest request)
    {
        if (!await _teams.IsUserInTeamAsync(userId, teamId)) throw new UnauthorizedAccessException("Not a member of the team.");
        if (request.AssignedToUserId.HasValue && !await _teams.IsUserInTeamAsync(request.AssignedToUserId.Value, teamId))
            throw new InvalidOperationException("Assignee must be a member of the team.");

        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            TeamId = teamId,
            CreatedByUserId = userId,
            AssignedToUserId = request.AssignedToUserId
        };
        await _tasks.AddAsync(task);
        await _tasks.SaveChangesAsync();
        return _mapper.Map<TaskResponse>(task);
    }

    public async Task<TaskResponse> UpdateTaskAsync(Guid taskId, Guid userId, UpdateTaskRequest request)
    {
        var existing = await _tasks.GetByIdAsync(taskId) ?? throw new KeyNotFoundException("Task not found.");
        if (!await _teams.IsUserInTeamAsync(userId, existing.TeamId)) throw new UnauthorizedAccessException("Not a member of the team.");

        if (request.Title != null) existing.Title = request.Title;
        if (request.Description != null) existing.Description = request.Description;
        if (request.DueDate.HasValue) existing.DueDate = request.DueDate;
        if (request.AssignedToUserId.HasValue)
        {
            if (!await _teams.IsUserInTeamAsync(request.AssignedToUserId.Value, existing.TeamId))
                throw new InvalidOperationException("Assignee must be a member of the team.");
            existing.AssignedToUserId = request.AssignedToUserId;
        }

        await _tasks.SaveChangesAsync();
        return _mapper.Map<TaskResponse>(existing);
    }

    public async Task DeleteTaskAsync(Guid taskId, Guid userId)
    {
        var existing = await _tasks.GetByIdAsync(taskId) ?? throw new KeyNotFoundException("Task not found.");
        if (!await _teams.IsUserInTeamAsync(userId, existing.TeamId)) throw new UnauthorizedAccessException("Not a member of the team.");
        await _tasks.DeleteAsync(existing);
        await _tasks.SaveChangesAsync();
    }

    public async Task<TaskResponse> PatchStatusAsync(Guid taskId, Guid userId, StatusPatchRequest request)
    {
        var existing = await _tasks.GetByIdAsync(taskId) ?? throw new KeyNotFoundException("Task not found.");
        if (!await _teams.IsUserInTeamAsync(userId, existing.TeamId)) throw new UnauthorizedAccessException("Not a member of the team.");
        existing.Status = request.Status;
        await _tasks.SaveChangesAsync();
        return _mapper.Map<TaskResponse>(existing);
    }
}
