using AutoMapper;
using TeamTask_API.Domain.Entities;
using TeamTask_API.DTOs.Common;
using TeamTask_API.DTOs.Tasks;
using TeamTask_API.DTOs.Tasks;
namespace TeamTaskApi.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<TaskItem, TaskResponse>();
    }
}
