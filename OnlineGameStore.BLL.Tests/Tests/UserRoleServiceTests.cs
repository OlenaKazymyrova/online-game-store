using AutoMapper;
using OnlineGameStore.BLL.Mapping.Profiles;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Tests.Tests;

public class UserRoleServiceTests
{
    private readonly UserRoleService _userRoleService;
    private readonly List<UserRole> _data;

    public UserRoleServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BllUserMappingProfile>();
            cfg.AddProfile<BllRoleMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var roleGen = new RoleEntityGenerator();
        var userGen = new UserEntityGenerator();
        var userRoleGen = new UserRoleEntityGenerator();

        var userData = userGen.Generate(100);
        var roleData = roleGen.Generate(100);

        _data = userRoleGen.Generate(100, userData, roleData);

        var userRoleRepository = new UserRoleRepositoryMockCreator(_data).Create();

        var userRepository = new UserRepositoryMockCreator(userData).Create();
        var roleRepository = new RoleRepositoryMockCreator(roleData).Create();

        _userRoleService = new UserRoleService(userRoleRepository, userRepository, roleRepository, mapper);
    }

    [Fact]
    public async Task GetUserRolesAsync_UserRolesExists_ReturnsUserRoles()
    {
        var userRole = _data[0];

        var expectedRoles = await _userRoleService.GetUserRolesAsync(userRole.UserId);

        Assert.NotEmpty(expectedRoles);
    }

    [Fact]
    public async Task GetUserRolesAsync_UserRolesDoesNotExist_ReturnsEmptyList()
    {
        var nonExistentUserId = Guid.NewGuid();

        var result = await _userRoleService.GetUserRolesAsync(nonExistentUserId);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUsersByRoleAsync_UserRolesExists_ReturnsUsersWithRole()
    {
        var userRole = _data[0];

        var expectedUsers = await _userRoleService.GetUsersByRoleAsync(userRole.RoleId);

        Assert.NotEmpty(expectedUsers);
    }

    [Fact]
    public async Task GetUsersByRoleAsync_UserRolesDoesNotExist_ReturnsEmptyList()
    {
        var nonExistentRoleId = Guid.NewGuid();

        var result = await _userRoleService.GetUsersByRoleAsync(nonExistentRoleId);

        Assert.Empty(result);
    }

    [Fact]
    public async Task UserHasRoleAsync_UserHasRole_ReturnsTrue()
    {
        var userRole = _data[0];

        var result = await _userRoleService.UserHasRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.True(result);
    }

    [Fact]
    public async Task UserHasRoleAsync_UserDoesNotHaveRole_ReturnsFalse()
    {
        var userId = _data[1].UserId;
        var roleId = _data[0].RoleId;

        var result = await _userRoleService.UserHasRoleAsync(userId, roleId);

        Assert.False(result);
    }

    [Fact]
    public async Task UserHasRoleAsync_NonExistingUser_ThrowsException()
    {
        var userRole = _data[0];
        var nonExistentUserId = Guid.NewGuid();

        await Assert.ThrowsAnyAsync<Exception>(async () =>
            await _userRoleService.AddUserRoleAsync(nonExistentUserId, userRole.RoleId));
    }

    [Fact]
    public async Task UserHasRoleAsync_NonExistingRole_ThrowsKeyNotFoundException()
    {
        var userRole = _data[0];
        var nonExistentRoleId = Guid.NewGuid();

        await Assert.ThrowsAnyAsync<Exception>(async () =>
            await _userRoleService.AddUserRoleAsync(userRole.UserId, nonExistentRoleId));
    }

    [Fact]
    public async Task UserHasRoleAsync_UserIdAndRoleIdAreEqual_ThrowsArgumentException()
    {
        var userRole = _data[0];

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userRoleService.UserHasRoleAsync(userRole.UserId, userRole.UserId));
    }

    [Fact]
    public async Task UserHasRoleAsync_UserIdOrRoleIdIsEmpty_ThrowsArgumentException()
    {
        var userRole = _data[0];

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userRoleService.UserHasRoleAsync(Guid.Empty, userRole.RoleId));

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userRoleService.UserHasRoleAsync(userRole.UserId, Guid.Empty));
    }

    [Fact]
    public async Task AddUserRoleAsync_ValidUserAndRole_AddsUserRole()
    {
        var userId = _data[0].UserId;
        var roleId = _data[1].RoleId;

        var result = await _userRoleService.AddUserRoleAsync(userId, roleId);

        Assert.True(result);
    }

    [Fact]
    public async Task AddUserRoleAsync_UserAlreadyHasRole_ReturnsFalse()
    {
        var userRole = _data[0];

        var result = await _userRoleService.AddUserRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.False(result);
    }

    [Fact]
    public async Task AddUserRoleAsync_InvalidUserOrRole_ThrowsKeyNotFoundException()
    {
        var nonExistentUserId = Guid.NewGuid();
        var nonExistentRoleId = Guid.NewGuid();

        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _userRoleService.AddUserRoleAsync(nonExistentUserId, nonExistentRoleId));
    }

    [Fact]
    public async Task AddUserRoleAsync_UserIdAndRoleIdAreEqual_ThrowsArgumentException()
    {
        var userRole = _data[0];

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userRoleService.AddUserRoleAsync(userRole.UserId, userRole.UserId));
    }

    [Fact]
    public async Task AddUserRoleAsync_UserIdOrRoleIdIsEmpty_ThrowsArgumentException()
    {
        var userRole = _data[0];

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userRoleService.AddUserRoleAsync(Guid.Empty, userRole.RoleId));

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userRoleService.AddUserRoleAsync(userRole.UserId, Guid.Empty));
    }

    [Fact]
    public async Task RemoveUserRoleAsync_ValidUserAndRole_RemovesUserRole()
    {
        var userRole = _data[0];

        var result = await _userRoleService.RemoveUserRoleAsync(userRole.UserId, userRole.RoleId);

        Assert.True(result);
    }

    [Fact]
    public async Task RemoveUserRoleAsync_UserDoesNotHaveRole_ReturnsFalse()
    {
        var userId = _data[1].UserId;
        var roleId = _data[0].RoleId;

        var result = await _userRoleService.RemoveUserRoleAsync(userId, roleId);

        Assert.False(result);
    }

    [Fact]
    public async Task RemoveUserRoleAsync_InvalidUserOrRole_ThrowsKeyNotFoundException()
    {
        var nonExistentUserId = Guid.NewGuid();
        var nonExistentRoleId = Guid.NewGuid();

        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _userRoleService.RemoveUserRoleAsync(nonExistentUserId, nonExistentRoleId));
    }

    [Fact]
    public async Task RemoveUserRoleAsync_UserIdAndRoleIdAreEqual_ThrowsArgumentException()
    {
        var userRole = _data[0];

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userRoleService.RemoveUserRoleAsync(userRole.UserId, userRole.UserId));
    }

    [Fact]
    public async Task RemoveUserRoleAsync_UserIdOrRoleIdIsEmpty_ThrowsArgumentException()
    {
        var userRole = _data[0];

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userRoleService.RemoveUserRoleAsync(Guid.Empty, userRole.RoleId));

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userRoleService.RemoveUserRoleAsync(userRole.UserId, Guid.Empty));
    }
}