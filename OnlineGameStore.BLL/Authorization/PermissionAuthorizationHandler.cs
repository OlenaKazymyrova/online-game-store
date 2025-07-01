using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OnlineGameStore.BLL.Authentication;
using OnlineGameStore.BLL.Exceptions;
using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.BLL.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(
            c => c.Type == CustomClaims.UserId);

        if (userId is null || !Guid.TryParse(userId.Value, out var id))
        {
            throw new UnauthorizedException("User is not authenticated or user ID claim is invalid.");
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var permissionService = scope.ServiceProvider
            .GetRequiredService<IPermissionService>();

        var permissions = await permissionService.GetPermissionAsync(id);

        if (!requirement.Permissions.All(p => permissions.Contains(p)))
        {
            throw new ForbiddenException("Missed required permissions.");
        }

        context.Succeed(requirement);

    }
}