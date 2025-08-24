using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using Microsoft.Extensions.Options;
using TeamTask_API.Domain.Repositories;
using TeamTask_API.Infrastructure.Services;
using TeamTask_API.Infrastructure.Security;
using TeamTask_API.Domain.Entities;
using TeamTask_API.DTOs.Common;
using System.Threading.Tasks;
using System;

namespace TeamTask_API_Tests.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAndLogin_Should_Return_Token()
    {
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        userRepo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        userRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
        userRepo.Setup(r => r.GetByEmailAsync("john@example.com")).ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "john@example.com", FullName = "John", PasswordHash = BCrypt.Net.BCrypt.HashPassword("secret") });

        var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserResponse>()).CreateMapper();
        var jwt = Options.Create(new JwtSettings { Issuer = "x", Audience = "y", Secret = "12345678901234567890123456789012", ExpiryMinutes = 60 });

        var svc = new AuthService(userRepo.Object, jwt, mapper);
        var reg = await svc.RegisterAsync("a@a.com", "A", "p@ss");
        reg.Token.Should().NotBeNullOrEmpty();

        var login = await svc.LoginAsync("john@example.com", "secret");
        login.Token.Should().NotBeNullOrEmpty();
    }
}
