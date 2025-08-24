using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTask_API.Domain.Services;

namespace TeamTask_API.Controllers;

[ApiController]
[Route("teams")]
[Authorize]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teams;
    public TeamsController(ITeamService teams) { _teams = teams; }

    [HttpPost]
    public async Task<ActionResult<object>> CreateTeam([FromBody] string name)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var teamId = await _teams.CreateTeamAsync(userId, name);
        return CreatedAtAction(nameof(CreateTeam), new { id = teamId }, new { id = teamId, name });
    }

    [HttpPost("{teamId:guid}/users")]
    public async Task<IActionResult> AddUser(Guid teamId, [FromBody] Guid userIdToAdd)
    {
        var adminUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _teams.AddUserToTeamAsync(teamId, adminUserId, userIdToAdd);
        return NoContent();
    }
}
