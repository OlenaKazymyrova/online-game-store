using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.DAL.Interfaces;

public interface IPermissionRepository
{
    Task<HashSet<PermissionEnum>> GetPermissionsAsync(Guid userId);
}