using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Enums;
using OnlineGameStore.SharedLogic.Settings;

namespace OnlineGameStore.DAL.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly OnlineGameStoreDbContext _dbContext;
    protected readonly DbSet<Permission> _dbSet;

    public PermissionRepository(OnlineGameStoreDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<Permission>();
    }
    public async Task<HashSet<PermissionEnum>> GetPermissionsAsync(Guid userId)
    {
        var permissionIds = await _dbSet
            .Where(p => p.RolePermissions
                .Any(rp => rp.Role.UserRoles
                    .Any(ur => ur.UserId == userId)))
            .Select(p => p.Id)
            .ToListAsync();

        var permissions = permissionIds
            .Select(SystemPermissionSettings.GetPermissionFromGuid)
            .ToHashSet();

        return permissions;
    }
}