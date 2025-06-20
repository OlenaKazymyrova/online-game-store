using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Interfaces;

public interface IUserRoleRepository
{
    Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId);
    Task<IEnumerable<User>> GetUsersByRoleAsync(Guid roleId);
    Task<bool> AddUserRoleAsync(Guid userId, Guid roleId);
    Task<bool> RemoveUserRoleAsync(Guid userId, Guid roleId);
    Task<bool> UserHasRoleAsync(Guid userId, Guid roleId);
}