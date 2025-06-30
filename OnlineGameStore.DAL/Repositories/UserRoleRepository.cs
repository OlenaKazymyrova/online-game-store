using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Constants;
using OnlineGameStore.SharedLogic.Enums;
using OnlineGameStore.SharedLogic.Settings;

namespace OnlineGameStore.DAL.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly OnlineGameStoreDbContext _dbContext;
    protected readonly DbSet<UserRole> _dbSet;

    public UserRoleRepository(OnlineGameStoreDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<UserRole>();
    }

    public async Task<HashSet<PermissionEnum>> GetUserRolesAsync(Guid userId)
    {
        var roles = await _dbSet
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Select(ur => ur.Role)
            .ToListAsync();
        
        var permissions = roles
            .SelectMany(r => r.RolePermissions.Select(rp => rp.Permission))
            .Select(p => SystemPermissionSettings.GetPermissionFromGuid(p.Id))
            .ToHashSet();
        
        return permissions;
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(Guid roleId)
    {
        return await _dbContext.UserRoles
            .Where(ur => ur.RoleId == roleId)
            .Select(ur => ur.User)
            .ToListAsync();
    }
    public async Task<bool> UserHasRoleAsync(Guid userId, Guid roleId)
    {
        return await _dbContext.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }

    public async Task<bool> AddUserRoleAsync(Guid userId, Guid roleId)
    {
        if (await UserHasRoleAsync(userId, roleId))
            return false;

        var userRole = new UserRole { UserId = userId, RoleId = roleId };
        _dbContext.UserRoles.Add(userRole);

        return await SaveChangesAsync();
    }

    public async Task<bool> RemoveUserRoleAsync(Guid userId, Guid roleId)
    {
        var userRole = await _dbContext.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (userRole == null)
            return false;

        _dbContext.UserRoles.Remove(userRole);

        return await SaveChangesAsync();
    }

    private async Task<bool> SaveChangesAsync()
    {
        try
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error saving changes: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error saving changes: {ex.Message}");
            throw;
        }
    }
}