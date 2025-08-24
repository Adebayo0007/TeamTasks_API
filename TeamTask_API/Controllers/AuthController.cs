using Microsoft.AspNetCore.Mvc;
using TeamTask_API.DTOs.Auth;
using TeamTask_API.Domain.Services;

namespace TeamTask_API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) { _auth = auth; }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var result = await _auth.RegisterAsync(request.Email, request.FullName, request.Password);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var result = await _auth.LoginAsync(request.Email, request.Password);
        return Ok(result);
    }
}
