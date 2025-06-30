using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.DAL.Interfaces;

public interface IUserRoleRepository
{
    Task<HashSet<PermissionEnum>> GetUserRolesAsync(Guid userId);
    Task<IEnumerable<User>> GetUsersByRoleAsync(Guid roleId);
    Task<bool> AddUserRoleAsync(Guid userId, Guid roleId);
    Task<bool> RemoveUserRoleAsync(Guid userId, Guid roleId);
    Task<bool> UserHasRoleAsync(Guid userId, Guid roleId);
}