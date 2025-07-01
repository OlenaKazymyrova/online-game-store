using Microsoft.AspNetCore.Authorization;
using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.BLL.Authorization;

public class PermissionRequirement(ICollection<PermissionEnum> permissions) : IAuthorizationRequirement
{
    public ICollection<PermissionEnum> Permissions { get; set; } = permissions;
}