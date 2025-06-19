using OnlineGameStore.BLL.DTOs;

namespace OnlineGameStore.BLL.Interfaces;

public interface IUserRoleService
{
    public Task<IEnumerable<RoleReadDto>> GetUserRolesAsync(Guid userId);
    public Task<bool> UserHasRoleAsync(Guid userId, Guid roleId);
    public Task<bool> AddUserRoleAsync(Guid userId, Guid roleId);
    public Task<bool> RemoveUserRoleAsync(Guid userId, Guid roleId);
}