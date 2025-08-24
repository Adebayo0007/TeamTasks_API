using TeamTask_API.Domain.Enums;
namespace TeamTask_API.DTOs.Tasks;
public class UpdateTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? AssignedToUserId { get; set; }
}