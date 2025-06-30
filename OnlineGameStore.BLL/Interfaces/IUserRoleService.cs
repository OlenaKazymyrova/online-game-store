using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.DTOs.Roles;
using OnlineGameStore.BLL.DTOs.Users;

namespace OnlineGameStore.BLL.Interfaces;

public interface IUserRoleService
{
    public Task<IEnumerable<RoleReadDto>> GetUserRolesAsync(Guid userId);
    public Task<IEnumerable<UserReadDto>> GetUsersByRoleAsync(Guid roleId);
    public Task<bool> UserHasRoleAsync(Guid userId, Guid roleId);
    public Task<bool> AddUserRoleAsync(Guid userId, Guid roleId);
    public Task<bool> RemoveUserRoleAsync(Guid userId, Guid roleId);
}