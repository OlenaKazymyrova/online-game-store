using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Tests.RepositoryCreators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class RoleRepositoryTests
{
    private readonly RoleRepositoryCreator _creator = new();

    [Fact]
    public async Task GetAsync_ReturnsAllRoles()
    {
        var repository = _creator.Create();
        var role1 = GetRole("Role 1", "Description 1");
        var role2 = GetRole("Role 2", "Description 2");

        await repository.AddAsync(role1);
        await repository.AddAsync(role2);

        var result = await repository.GetAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
        Assert.Contains(result.Items, r => r.Name == role1.Name);
        Assert.Contains(result.Items, r => r.Name == role2.Name);
    }

    [Fact]
    public async Task ExistsAsync_RoleExists_ReturnsTrue()
    {
        var repository = _creator.Create();
        var role = GetRole();

        var addedRole = await repository.AddAsync(role);

        Assert.NotNull(addedRole);

        var result = await repository.ExistsAsync(addedRole!.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_RoleDoesNotExist_ReturnsFalse()
    {
        var repository = _creator.Create();

        var result = await repository.ExistsAsync(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public async Task GetByIdAsync_RoleExists_ReturnsRole()
    {
        var repository = _creator.Create();
        var role = GetRole();

        var addedRole = await repository.AddAsync(role);

        Assert.NotNull(addedRole);

        var result = await repository.GetByIdAsync(addedRole!.Id);

        Assert.NotNull(result);
        Assert.Equal(addedRole, result);
    }

    [Fact]
    public async Task GetByIdAsync_RoleDoesNotExits_ReturnsNull()
    {
        var repository = _creator.Create();
        var role = GetRole();

        var result = await repository.GetByIdAsync(role.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsRole()
    {
        var repository = _creator.Create();
        var role = GetRole();
        var result = await repository.AddAsync(role);

        Assert.NotNull(result);
        Assert.True(result.Id != Guid.Empty);
        Assert.Equal(role, result);
    }

    [Fact]
    public async Task UpdateAsync_RoleExists_UpdatesRole()
    {
        var repository = _creator.Create();
        var role = GetRole();
        const string newName = "Updated Role Name";
        const string newDescription = "Updated Description";

        var addedRole = await repository.AddAsync(role);

        Assert.NotNull(addedRole);

        addedRole.Name = newName;
        addedRole.Description = newDescription;

        var result = await repository.UpdateAsync(addedRole);

        Assert.True(result);

        var updatedRole = await repository.GetByIdAsync(addedRole.Id);

        Assert.NotNull(updatedRole);
        Assert.Equal(newName, updatedRole!.Name);
        Assert.Equal(newDescription, updatedRole.Description);
    }

    [Fact]
    public async Task UpdateAsync_RoleDoesNotExist_ReturnsException()
    {
        var repository = _creator.Create();
        var role = GetRole();

        await Assert.ThrowsAnyAsync<Exception>(async () => await repository.UpdateAsync(role));
    }

    [Fact]
    public async Task DeleteAsync_RoleExists_DeletesRole()
    {
        var repository = _creator.Create();
        var role = GetRole();

        var addedRole = await repository.AddAsync(role);

        Assert.NotNull(addedRole);

        var result = await repository.DeleteAsync(addedRole.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_RoleDoesNotExist_DoesNothing()
    {
        var repository = _creator.Create();

        var result = await repository.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }

    private static Role GetRole(
        string name = "Test Role",
        string description = "Test Description")
    {
        return new Role
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };
    }
}