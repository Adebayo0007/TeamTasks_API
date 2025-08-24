using TeamTask_API.Domain.Enums;
namespace TeamTask_API.DTOs.Tasks;
public class StatusPatchRequest
{
    public Status Status { get; set; }
}