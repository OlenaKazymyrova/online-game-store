using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Repositories;
using OnlineGameStore.DAL.Tests.RepositoryCreators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class UserRoleRepositoryTests
{
    private readonly UserRoleRepositoryCreator _creator = new();

    [Fact]
    public async Task GetUserRolesAsync_UserHasRoles_ReturnsRoles()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new OnlineGameStoreDbContext(options);

        var userRole = GetUserRole();

        var role = new Role
        {
            Id = userRole.RoleId,
            Name = "TestRole"
        };

        await dbContext.Roles.AddAsync(role);
        await dbContext.SaveChangesAsync();

        var repository = new UserRoleRepository(dbContext);

        var addedUserRole = await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.True(addedUserRole);

        var roles = await repository.GetUserRolesAsync(userRole.UserId);

        Assert.NotNull(roles);
        Assert.Contains(userRole.RoleId, roles.Select(r => r.Id));
    }

    [Fact]
    public async Task UserHasRoleAsync_RoleExists_ReturnsTrue()
    {
        var repository = _creator.Create();
        var userRole = GetUserRole();

        await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        var hasRole = await repository.UserHasRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.True(hasRole);
    }

    [Fact]
    public async Task UserHasRoleAsync_RoleDoesNotExist_ReturnsFalse()
    {
        var repository = _creator.Create();
        var userRole = GetUserRole();

        var hasRole = await repository.UserHasRoleAsync(userRole.UserId, Guid.NewGuid());

        Assert.False(hasRole);
    }

    [Fact]
    public async Task AddUserRoleAsync_RelationDoesNotExist_ReturnsTrue()
    {
        var repository = _creator.Create();
        var userRole = GetUserRole();

        var added = await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.True(added);
    }

    [Fact]
    public async Task AddUserRoleAsync_RelationExists_ReturnsFalse()
    {
        var repository = _creator.Create();
        var userRole = GetUserRole();

        var added = await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.True(added);

        var addedAgain = await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.False(addedAgain);
    }

    [Fact]
    public async Task RemoveUserRoleAsync_RelationExists_ReturnsTrue()
    {
        var repository = _creator.Create();
        var userRole = GetUserRole();

        await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        var removed = await repository.RemoveUserRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.True(removed);
    }

    [Fact]
    public async Task RemoveUserRoleAsync_RelationDoesNotExist_ReturnsFalse()
    {
        var repository = _creator.Create();
        var userRole = GetUserRole();

        var removed = await repository.RemoveUserRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.False(removed);
    }

    private static UserRole GetUserRole(Guid? userId = null, Guid? roleId = null)
    {
        return new UserRole
        {
            UserId = userId ?? Guid.NewGuid(),
            RoleId = roleId ?? Guid.NewGuid()
        };
    }
}