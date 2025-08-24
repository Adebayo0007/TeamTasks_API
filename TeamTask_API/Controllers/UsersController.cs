using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTask_API.Domain.Services;
using TeamTask_API.DTOs.Common;

namespace TeamTask_API.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IAuthService _auth;
    public UsersController(IAuthService auth) { _auth = auth; }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var me = await _auth.GetMeAsync(Guid.Parse(userId!));
        return Ok(me);
    }
}
