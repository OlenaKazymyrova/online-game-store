using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.SharedLogic.Enums;

public class PermissionServiceMock : IPermissionService
{
    public Task<HashSet<PermissionEnum>> GetPermissionAsync(Guid userId)
    {
        return Task.FromResult(new HashSet<PermissionEnum> { PermissionEnum.Read, PermissionEnum.Create, PermissionEnum.Delete, PermissionEnum.Update });
    }
}