using Microsoft.AspNetCore.Authorization;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.BLL.Infrastracture;

public class PermissionRequirement(ICollection<PermissionEnum> permissions) : IAuthorizationRequirement
{
    public ICollection<PermissionEnum> Permissions { get; set; } = permissions;
}