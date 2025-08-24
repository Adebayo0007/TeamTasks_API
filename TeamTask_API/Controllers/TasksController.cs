using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTask_API.Domain.Services;
using TeamTask_API.DTOs.Tasks;

namespace TeamTask_API.Controllers;

[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskService _tasks;
    public TasksController(ITaskService tasks) { _tasks = tasks; }

    [HttpGet("teams/{teamId:guid}/tasks")]
    [Authorize(Policy = "TeamMember")]
    public async Task<ActionResult<List<TaskResponse>>> GetTeamTasks(Guid teamId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var tasks = await _tasks.GetTeamTasksAsync(teamId, userId);
        return Ok(tasks);
    }

    [HttpPost("teams/{teamId:guid}/tasks")]
    [Authorize(Policy = "TeamMember")]
    public async Task<ActionResult<TaskResponse>> Create(Guid teamId, CreateTaskRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var created = await _tasks.CreateTaskAsync(teamId, userId, request);
        return CreatedAtAction(nameof(GetTeamTasks), new { teamId }, created);
    }

    [HttpPut("tasks/{taskId:guid}")]
    [Authorize]
    public async Task<ActionResult<TaskResponse>> Update(Guid taskId, UpdateTaskRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var updated = await _tasks.UpdateTaskAsync(taskId, userId, request);
        return Ok(updated);
    }

    [HttpDelete("tasks/{taskId:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid taskId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _tasks.DeleteTaskAsync(taskId, userId);
        return NoContent();
    }

    [HttpPatch("tasks/{taskId:guid}/status")]
    [Authorize]
    public async Task<ActionResult<TaskResponse>> PatchStatus(Guid taskId, StatusPatchRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var updated = await _tasks.PatchStatusAsync(taskId, userId, request);
        return Ok(updated);
    }
}
