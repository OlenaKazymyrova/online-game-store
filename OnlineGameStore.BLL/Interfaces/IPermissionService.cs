using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.BLL.Interfaces;

public interface IPermissionService
{
    Task<HashSet<PermissionEnum>> GetPermissionAsync(Guid userId);
}