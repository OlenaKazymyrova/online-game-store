using AutoMapper;
using OnlineGameStore.BLL.DTOs.Roles;
using OnlineGameStore.BLL.Exceptions;
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
    public async Task GetRoleAsync_RoleDoesNotExist_ThrowsNotFoundException()
    {
        var nonExistentRoleId = Guid.NewGuid();

        await Assert.ThrowsAsync<NotFoundException>(async () => await _roleService.GetByIdAsync(nonExistentRoleId));
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
    public async Task AddRoleAsync_InvalidRole_ThrowsValidationException()
    {
        RoleCreateDto? invalidRole = null;

        await Assert.ThrowsAsync<ValidationException>(async () => await _roleService.AddAsync(invalidRole!));
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
    public async Task UpdateRoleAsync_InvalidData_ThrowsValidationException()
    {
        var role = _data[0];

        RoleCreateDto? invalidRoleDto = null;

        await Assert.ThrowsAsync<ValidationException>(async () =>
            await _roleService.UpdateAsync(role.Id, invalidRoleDto!));
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
    public async Task DeleteRoleAsync_RoleExists_ThrowsNotFoundException()
    {
        var role = _data[0];

        var isDeleted = await _roleService.DeleteAsync(role.Id);

        Assert.True(isDeleted);

        await Assert.ThrowsAsync<NotFoundException>(async () => await _roleService.GetByIdAsync(role.Id));
    }

    [Fact]
    public async Task DeleteRoleAsync_RoleDoesNotExist_ReturnsFalse()
    {
        var nonExistentRoleId = Guid.NewGuid();

        var isDeleted = await _roleService.DeleteAsync(nonExistentRoleId);

        Assert.False(isDeleted);
    }
}