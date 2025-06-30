using OnlineGameStore.SharedLogic.Constants;
using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.SharedLogic.Settings;

public static class SystemPermissionSettings
{
    public static readonly Dictionary<RoleEnum, PermissionEnum[]> PermissionsByRole = new()
    {
        [RoleEnum.Admin] = new[]
        {
            PermissionEnum.Read,
            PermissionEnum.Create,
            PermissionEnum.Update,
            PermissionEnum.Delete
        },
        [RoleEnum.User] = new[] 
        { 
            PermissionEnum.Read 
        }
    };

    public static readonly Dictionary<RoleEnum, string> RoleDescriptions = new()
    {
        [RoleEnum.Admin] = "Administrator with full system access",
        [RoleEnum.User] = "Standard user with basic access"
    };

    public static readonly Guid ReadId = Guid.Parse("00000001-0000-0000-0000-000000000000");
    public static readonly Guid CreateId = Guid.Parse("00000002-0000-0000-0000-000000000000");
    public static readonly Guid UpdateId = Guid.Parse("00000003-0000-0000-0000-000000000000");
    public static readonly Guid DeleteId = Guid.Parse("00000004-0000-0000-0000-000000000000");
    
    
    public static readonly Guid AdminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid UserId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static Guid GetRoleGuid(RoleEnum role) =>
        role switch
        {
            RoleEnum.Admin => AdminId,
            RoleEnum.User => UserId
        };

    public static Guid GetPermissionGuid(PermissionEnum permission) => 
        permission switch
        {
            PermissionEnum.Read => ReadId,
            PermissionEnum.Create => CreateId,
            PermissionEnum.Update => UpdateId,
            PermissionEnum.Delete => DeleteId,
        };
    
    public static string GetRoleDescription(RoleEnum role) =>
        RoleDescriptions.TryGetValue(role, out var description) 
            ? description 
            : string.Empty;
    
    public static PermissionEnum GetPermissionFromGuid(Guid permissionId) =>
        permissionId switch
        {
            _ when permissionId == ReadId => PermissionEnum.Read,
            _ when permissionId == CreateId => PermissionEnum.Create,
            _ when permissionId == UpdateId => PermissionEnum.Update,
            _ when permissionId == DeleteId => PermissionEnum.Delete
        };

    
}