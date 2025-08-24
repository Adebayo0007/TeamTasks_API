using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TeamTask_API.Domain.Repositories;

namespace TeamTask_API.Infrastructure.Policies;

public class TeamMemberHandler : AuthorizationHandler<TeamMemberRequirement>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpContextAccessor _http;

    public TeamMemberHandler(IServiceProvider serviceProvider, IHttpContextAccessor http)
    {
        _serviceProvider = serviceProvider;
        _http = http;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TeamMemberRequirement requirement)
    {
        var routeValues = _http.HttpContext?.Request.RouteValues;
        if (routeValues == null || !routeValues.TryGetValue("teamId", out var teamIdObj))
            return;

        if (Guid.TryParse(teamIdObj?.ToString(), out var teamId))
        {
            var userIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdStr, out var userId))
            {
                // Create a scope to resolve scoped services (like ITeamRepository)
                using var scope = _serviceProvider.CreateScope();
                var teamRepo = scope.ServiceProvider.GetRequiredService<ITeamRepository>();

                if (await teamRepo.IsUserInTeamAsync(userId, teamId))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
