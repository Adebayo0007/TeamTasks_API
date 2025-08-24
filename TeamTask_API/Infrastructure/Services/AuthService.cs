using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using TeamTask_API.Domain.Entities;
using TeamTask_API.Domain.Repositories;
using TeamTask_API.Domain.Services;
using TeamTask_API.DTOs.Auth;
using TeamTask_API.DTOs.Common;
using TeamTask_API.Infrastructure.Security;
using System.Security.Claims;

namespace TeamTask_API.Infrastructure.Services;
public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwt;

    public AuthService(IUserRepository users, IOptions<JwtSettings> jwtOptions, IMapper mapper)
    {
        _users = users;
        _mapper = mapper;
        _jwt = jwtOptions.Value;
    }

    public async Task<AuthResponse> RegisterAsync(string email, string fullName, string password)
    {
        var existing = await _users.GetByEmailAsync(email);
        if (existing != null) throw new InvalidOperationException("Email already registered.");

        var user = new User
        {
            Email = email,
            FullName = fullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };
        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        return GenerateToken(user);
    }

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var user = await _users.GetByEmailAsync(email) ?? throw new UnauthorizedAccessException("Invalid credentials.");
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) throw new UnauthorizedAccessException("Invalid credentials.");
        return GenerateToken(user);
    }

    public async Task<UserResponse> GetMeAsync(Guid userId)
    {
        var user = await _users.GetByIdAsync(userId) ?? throw new KeyNotFoundException("User not found.");
        return _mapper.Map<UserResponse>(user);
    }

    private AuthResponse GenerateToken(User user)
    {
        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(_jwt.ExpiryMinutes);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new("fullName", user.FullName)
        };
        var token = JwtTokenHelper.GenerateToken(_jwt, claims, expires);
        return new AuthResponse { Token = token, ExpiresAt = expires };
    }
}
