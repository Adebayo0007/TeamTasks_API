using TeamTask_API.DTOs.Auth;
using TeamTask_API.DTOs.Common;
namespace TeamTask_API.Domain.Services;
public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(string email, string fullName, string password);
    Task<AuthResponse> LoginAsync(string email, string password);
    Task<UserResponse> GetMeAsync(Guid userId);
}