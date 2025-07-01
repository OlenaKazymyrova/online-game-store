using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.BLL.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public Task<HashSet<PermissionEnum>> GetPermissionAsync(Guid userId) =>
        _permissionRepository.GetPermissionsAsync(userId);
}