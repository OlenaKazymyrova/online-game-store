using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.DTOs.Roles;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.BLL.Interfaces;

public interface IUserRoleService
{
    Task<HashSet<PermissionEnum>> GetPermissionsAsync(Guid userId);
    public Task<IEnumerable<UserReadDto>> GetUsersByRoleAsync(Guid roleId);
    public Task<bool> UserHasRoleAsync(Guid userId, Guid roleId);
    public Task<bool> AddUserRoleAsync(Guid userId, Guid roleId);
    public Task<bool> RemoveUserRoleAsync(Guid userId, Guid roleId);
}