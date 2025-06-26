using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Tests.RepositoryCreators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class UserRepositoryTests
{
    private readonly UserRepositoryCreator _creator = new();

    [Fact]
    public async Task GetAsync_ReturnsAllUsers()
    {
        var repository = _creator.Create();
        var user1 = GetUser("User 1", "cool@example.com");
        var user2 = GetUser("User 2", "john@example.com");

        await repository.AddAsync(user1);
        await repository.AddAsync(user2);

        var result = await repository.GetAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
        Assert.Contains(result.Items, u => u.Username == user1.Username);
        Assert.Contains(result.Items, u => u.Username == user2.Username);
    }

    [Fact]
    public async Task ExistsAsync_UserExists_ReturnsTrue()
    {
        var repository = _creator.Create();
        var user = GetUser();

        var addedUser = await repository.AddAsync(user);

        Assert.NotNull(addedUser);

        var result = await repository.ExistsAsync(addedUser!.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_UserDoesNotExist_ReturnsFalse()
    {
        var repository = _creator.Create();

        var result = await repository.ExistsAsync(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public async Task GetByIdAsync_UserExists_ReturnsRole()
    {
        var repository = _creator.Create();
        var user = GetUser();

        var addedUser = await repository.AddAsync(user);

        Assert.NotNull(addedUser);

        var result = await repository.GetByIdAsync(addedUser!.Id);

        Assert.NotNull(result);
        Assert.Equal(addedUser, result);
    }

    [Fact]
    public async Task GetByIdAsync_UserDoesNotExits_ReturnsNull()
    {
        var repository = _creator.Create();
        var user = GetUser();

        var result = await repository.GetByIdAsync(user.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsUser()
    {
        var repository = _creator.Create();
        var user = GetUser();
        var result = await repository.AddAsync(user);

        Assert.NotNull(result);
        Assert.True(result.Id != Guid.Empty);
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task UpdateAsync_UserExists_UpdatesRole()
    {
        var repository = _creator.Create();
        var user = GetUser();
        const string newUsername = "UpdatedName";
        const string newEmail = "new@example.com";

        var addedUser = await repository.AddAsync(user);

        Assert.NotNull(addedUser);

        addedUser.Username = newUsername;
        addedUser.Email = newEmail;

        var result = await repository.UpdateAsync(addedUser);

        Assert.True(result);

        var updatedUser = await repository.GetByIdAsync(addedUser.Id);

        Assert.NotNull(updatedUser);
        Assert.Equal(newUsername, updatedUser!.Username);
        Assert.Equal(newEmail, updatedUser.Email);
    }

    [Fact]
    public async Task UpdateAsync_UserDoesNotExist_ReturnsException()
    {
        var repository = _creator.Create();
        var user = GetUser();

        await Assert.ThrowsAnyAsync<Exception>(async () => await repository.UpdateAsync(user));
    }

    [Fact]
    public async Task DeleteAsync_UserExists_DeletesRole()
    {
        var repository = _creator.Create();
        var user = GetUser();

        var addedUser = await repository.AddAsync(user);

        Assert.NotNull(addedUser);

        var result = await repository.DeleteAsync(addedUser.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_UserDoesNotExist_DoesNothing()
    {
        var repository = _creator.Create();

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.DeleteAsync(Guid.NewGuid()));
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
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }
}