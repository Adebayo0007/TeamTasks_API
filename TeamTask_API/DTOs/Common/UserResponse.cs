using TeamTask_API.Domain.Enums;
namespace TeamTask_API.DTOs.Common;
public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
}