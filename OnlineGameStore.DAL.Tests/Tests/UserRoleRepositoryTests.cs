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
        var userRole = GetUserRole();

        var dbContext = await GetSeededContext(userRole);

        var repository = new UserRoleRepository(dbContext);

        var addedUserRole = await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.True(addedUserRole);

        var roles = await repository.GetUserRolesAsync(userRole.UserId);

        Assert.NotNull(roles);
        Assert.Contains(userRole.RoleId, roles.Select(r => r.Id));
    }

    [Fact]
    public async Task GetUserRoleAsync_UserDeleted_ReturnsEmptyList()
    {
        var userRole = GetUserRole();

        var dbContext = await GetSeededContext(userRole);

        var repository = new UserRoleRepository(dbContext);

        await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        var rolesEnum = await repository.GetUserRolesAsync(userRole.UserId);
        var nonEmptyRoles = rolesEnum.ToList();

        Assert.NotEmpty(nonEmptyRoles);
        Assert.Contains(userRole.RoleId, nonEmptyRoles.Select(r => r.Id));

        var userRepository = new UserRepository(dbContext);
        await userRepository.DeleteAsync(userRole.UserId);

        var roles = await repository.GetUserRolesAsync(userRole.UserId);

        Assert.Empty(roles);
    }

    [Fact]
    public async Task GetUserRoleAsync_RoleDeleted_ReturnsEmptyList()
    {
        var userRole = GetUserRole();

        var dbContext = await GetSeededContext(userRole);

        var repository = new UserRoleRepository(dbContext);

        await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        var rolesEnum = await repository.GetUserRolesAsync(userRole.UserId);
        var nonEmptyRoles = rolesEnum.ToList();

        Assert.NotEmpty(nonEmptyRoles);
        Assert.Contains(userRole.RoleId, nonEmptyRoles.Select(r => r.Id));

        var roleRepository = new RoleRepository(dbContext);
        await roleRepository.DeleteAsync(userRole.RoleId);

        var roles = await repository.GetUserRolesAsync(userRole.UserId);

        Assert.Empty(roles);
    }

    [Fact]
    public async Task GetUsersByRoleAsync_UserHasRoles_ReturnsRoles()
    {
        var userRole = GetUserRole();

        var dbContext = await GetSeededContext(userRole);

        var repository = new UserRoleRepository(dbContext);

        var addedUserRole = await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.True(addedUserRole);

        var users = await repository.GetUsersByRoleAsync(userRole.RoleId);

        Assert.NotNull(users);
        Assert.Contains(userRole.UserId, users.Select(u => u.Id));
    }

    [Fact]
    public async Task GetUsersByRoleAsync_UserDeleted_ReturnsEmptyList()
    {
        var userRole = GetUserRole();

        var dbContext = await GetSeededContext(userRole);

        var repository = new UserRoleRepository(dbContext);

        await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        var usersEnum = await repository.GetUsersByRoleAsync(userRole.RoleId);
        var nonEmptyUsers = usersEnum.ToList();

        Assert.NotEmpty(nonEmptyUsers);
        Assert.Contains(userRole.UserId, nonEmptyUsers.Select(u => u.Id));

        var userRepository = new UserRepository(dbContext);
        await userRepository.DeleteAsync(userRole.UserId);

        var users = await repository.GetUsersByRoleAsync(userRole.RoleId);

        Assert.Empty(users);
    }

    [Fact]
    public async Task GetUsersByRoleAsync_RoleDeleted_ReturnsEmptyList()
    {
        var userRole = GetUserRole();

        var dbContext = await GetSeededContext(userRole);

        var repository = new UserRoleRepository(dbContext);

        await repository.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        var usersEnum = await repository.GetUsersByRoleAsync(userRole.RoleId);
        var nonEmptyUsers = usersEnum.ToList();

        Assert.NotEmpty(nonEmptyUsers);
        Assert.Contains(userRole.UserId, nonEmptyUsers.Select(u => u.Id));

        var roleRepository = new RoleRepository(dbContext);
        await roleRepository.DeleteAsync(userRole.RoleId);

        var users = await repository.GetUserRolesAsync(userRole.UserId);

        Assert.Empty(users);
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

    private static async Task<OnlineGameStoreDbContext> GetSeededContext(UserRole userRole)
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new OnlineGameStoreDbContext(options);

        var role = new Role
        {
            Id = userRole.RoleId,
            Name = "TestRole"
        };

        var user = new User
        {
            Id = userRole.UserId,
            Username = "TestUser",
            Email = "user@example.com",
            PasswordHash = "hashed_password"
        };

        await dbContext.Users.AddAsync(user);
        await dbContext.Roles.AddAsync(role);
        await dbContext.SaveChangesAsync();

        return dbContext;
    }
}