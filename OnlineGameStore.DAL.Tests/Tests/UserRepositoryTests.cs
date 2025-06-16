using Azure.Identity;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Tests.RepositoryCreators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class UserRepositoryTests
{
    private readonly UserRepositoryCreator _creator = new();

    [Fact]
    public async Task GetAsync_ReturnsAllRoles()
    {
        var repository = _creator.Create();
        var role1 = GetUser("User 1", "cool@example.com");
        var role2 = GetUser("User 2", "john@example.com");

        await repository.AddAsync(role1);
        await repository.AddAsync(role2);

        var result = await repository.GetAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
        Assert.Contains(result.Items, r => r.UserName == role1.UserName);
        Assert.Contains(result.Items, r => r.UserName == role2.UserName);
    }

    [Fact]
    public async Task ExistsAsync_RoleExists_ReturnsTrue()
    {
        var repository = _creator.Create();
        var role = GetUser();

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
        var role = GetUser();

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
        var role = GetUser();

        var result = await repository.GetByIdAsync(role.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsRole()
    {
        var repository = _creator.Create();
        var role = GetUser();
        var result = await repository.AddAsync(role);

        Assert.NotNull(result);
        Assert.True(result.Id != Guid.Empty);
        Assert.Equal(role, result);
    }

    [Fact]
    public async Task UpdateAsync_RoleExists_UpdatesRole()
    {
        var repository = _creator.Create();
        var role = GetUser();
        const string newUsername = "UpdatedName";
        const string newEmail = "new@example.com";

        var addedRole = await repository.AddAsync(role);

        Assert.NotNull(addedRole);

        addedRole.UserName = newUsername;
        addedRole.Email = newEmail;

        var result = await repository.UpdateAsync(addedRole);

        Assert.True(result);

        var updatedRole = await repository.GetByIdAsync(addedRole.Id);

        Assert.NotNull(updatedRole);
        Assert.Equal(newUsername, updatedRole!.UserName);
        Assert.Equal(newEmail, updatedRole.Email);
    }

    [Fact]
    public async Task UpdateAsync_RoleDoesNotExist_ReturnsException()
    {
        var repository = _creator.Create();
        var role = GetUser();

        await Assert.ThrowsAnyAsync<Exception>(async () => await repository.UpdateAsync(role));
    }

    [Fact]
    public async Task DeleteAsync_RoleExists_DeletesRole()
    {
        var repository = _creator.Create();
        var role = GetUser();

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

    private static User GetUser(
        string username = "TestUser",
        string email = "user@example.com",
        string passwordHash = "hashedPassword",
        DateTime createdAt = default,
        DateTime? updatedAt = null)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }
}