using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.BLL.Infrastracture;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(
            c => c.Type == CustomClaims.UserId);
        if (userId is null || !Guid.TryParse(userId.Value, out var id))
        {
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var userRoleService = scope.ServiceProvider
            .GetRequiredService<IUserRoleService>();

        var permissions = await userRoleService.GetPermissionsAsync(id);

        if (requirement.Permissions.All(p => permissions.Contains(p)))
        {
            context.Succeed(requirement);
        }
    }
}