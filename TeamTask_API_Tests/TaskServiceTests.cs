using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using TeamTask_API.Domain.Repositories;
using TeamTask_API.Infrastructure.Services;
using TeamTask_API.Domain.Entities;
using TeamTask_API.DTOs.Tasks;
using TeamTask_API.DTOs.Common;
using System.Threading.Tasks;
using System;

namespace TeamTask_API_Tests.Tests;

public class TaskServiceTests
{
    [Fact]
    public async Task GetTeamTasks_Should_Throw_When_NotMember()
    {
        var tasksRepo = new Mock<ITaskRepository>();
        var teamRepo = new Mock<ITeamRepository>();
        var userRepo = new Mock<IUserRepository>();
        var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TaskItem, TaskResponse>()).CreateMapper();

        teamRepo.Setup(r => r.IsUserInTeamAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);

        var svc = new TaskService(tasksRepo.Object, teamRepo.Object, userRepo.Object, mapper);
        Func<Task> act = async () => await svc.GetTeamTasksAsync(Guid.NewGuid(), Guid.NewGuid());
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
