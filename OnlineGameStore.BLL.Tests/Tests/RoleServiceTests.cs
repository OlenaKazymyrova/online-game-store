using AutoMapper;
using OnlineGameStore.BLL.DTOs.Roles;
using OnlineGameStore.BLL.Mapping.Profiles;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Tests.Tests;

public class RoleServiceTests
{
    private readonly RoleService _roleService;
    private readonly List<Role> _data;

    public RoleServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BllRoleMappingProfile>());
        var mapper = config.CreateMapper();

        var rolesGen = new RoleEntityGenerator();
        _data = rolesGen.Generate(100);

        var repoMock = new RoleRepositoryMockCreator(_data);
        var mockRepository = repoMock.Create();

        _roleService = new RoleService(mockRepository, mapper);
    }

    [Fact]
    public async Task GetRoleAsync_RoleExists_ReturnsRole()
    {
        var role = _data[0];

        var expectedRole = await _roleService.GetByIdAsync(role.Id);

        Assert.NotNull(expectedRole);
        Assert.Equal(role.Name, expectedRole.Name);
    }

    [Fact]
    public async Task GetRoleAsync_RoleDoesNotExist_ReturnsNull()
    {
        var nonExistentRoleId = Guid.NewGuid();

        var result = await _roleService.GetByIdAsync(nonExistentRoleId);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddRoleAsync_ValidRole_AddsRole()
    {
        var newRole = new RoleCreateDto
        {
            Name = "Test role",
            Description = "Test description"
        };

        var addedRole = await _roleService.AddAsync(newRole);

        Assert.NotNull(addedRole);
        Assert.Equal(newRole.Name, addedRole.Name);
    }

    [Fact]
    public async Task AddRoleAsync_InvalidRole_ReturnsNull()
    {
        RoleCreateDto? invalidRole = null;

        var addedRole = await _roleService.AddAsync(invalidRole);

        Assert.Null(addedRole);
    }

    [Fact]
    public async Task UpdateRoleAsync_ValidData_ReturnsTrue()
    {
        var role = _data[0];

        var updatedRoleDto = new RoleCreateDto
        {
            Name = "Test role",
            Description = "Test description"
        };

        var isUpdated = await _roleService.UpdateAsync(role.Id, updatedRoleDto);

        Assert.True(isUpdated);

        var updatedRole = await _roleService.GetByIdAsync(role.Id);

        Assert.NotNull(updatedRole);
        Assert.Equal(updatedRoleDto.Name, updatedRole.Name);
    }

    [Fact]
    public async Task UpdateRoleAsync_InvalidData_ReturnsFalse()
    {
        var role = _data[0];

        RoleCreateDto? invalidRoleDto = null;

        var isUpdated = await _roleService.UpdateAsync(role.Id, invalidRoleDto);

        Assert.False(isUpdated);
    }

    [Fact]
    public async Task UpdateRoleAsync_RoleDoesNotExist_ReturnsFalse()
    {
        var nonExistentRoleId = Guid.NewGuid();

        var roleDto = new RoleCreateDto
        {
            Name = "Test role",
            Description = "Test description"
        };

        var isUpdated = await _roleService.UpdateAsync(nonExistentRoleId, roleDto);

        Assert.False(isUpdated);
    }

    [Fact]
    public async Task DeleteRoleAsync_RoleExists_ReturnsTrue()
    {
        var role = _data[0];

        var isDeleted = await _roleService.DeleteAsync(role.Id);

        Assert.True(isDeleted);

        var deletedRole = await _roleService.GetByIdAsync(role.Id);

        Assert.Null(deletedRole);
    }

    [Fact]
    public async Task DeleteRoleAsync_RoleDoesNotExist_ReturnsFalse()
    {
        var nonExistentRoleId = Guid.NewGuid();

        var isDeleted = await _roleService.DeleteAsync(nonExistentRoleId);

        Assert.False(isDeleted);
    }
}