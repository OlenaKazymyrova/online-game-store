using AutoMapper;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Mapping.Profiles;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.Tests;

public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly List<User> _data;

    public UserServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BllUserMappingProfile>());
        var mapper = config.CreateMapper();

        var userGen = new UserEntityGenerator();
        _data = userGen.Generate(100);

        var repoMock = new UserRepositoryMockCreator(_data);
        var mockRepository = repoMock.Create();

        _userService = new UserService(mockRepository, mapper);
    }

    [Fact]
    public async Task GetUserAsync_UserExists_ReturnUser()
    {
        var user = _data[0];

        var expectedUser = await _userService.GetByIdAsync(user.Id);

        Assert.NotNull(expectedUser);
        Assert.Equal(user.Username, expectedUser.Username);
    }

    [Fact]
    public async Task GetUserAsync_UserDoesNotExist_ReturnsNull()
    {
        var nonExistentUserId = Guid.NewGuid();

        var result = await _userService.GetByIdAsync(nonExistentUserId);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddUserAsync_ValidUser_AddsUser()
    {
        var newUser = new UserCreateDto
        {
            Username = "NewUser",
            Email = "test@example.com",
            Password = "hashedpassword"
        };

        var addedUser = await _userService.AddAsync(newUser);

        Assert.NotNull(addedUser);
        Assert.Equal(newUser.Username, addedUser.Username);
    }

    [Fact]
    public async Task AddUserAsync_InvalidUser_ReturnsNull()
    {
        UserCreateDto? invalidUser = null;

        var addedUser = await _userService.AddAsync(invalidUser);

        Assert.Null(addedUser);
    }

    [Fact]
    public async Task UpdateUserAsync_ValidData_ReturnsTrue()
    {
        var user = _data[0];

        var updatedUserDto = new UserCreateDto
        {
            Username = "UpdatedUser",
            Email = user.Email,
            Password = user.PasswordHash
        };

        var isUpdated = await _userService.UpdateAsync(user.Id, updatedUserDto);

        Assert.True(isUpdated);

        var updatedUser = await _userService.GetByIdAsync(user.Id);

        Assert.NotNull(updatedUser);
        Assert.Equal(updatedUserDto.Username, updatedUser.Username);
    }

    [Fact]
    public async Task UpdateUserAsync_InvalidData_ReturnsFalse()
    {
        var user = _data[0];

        UserCreateDto? invalidUserDto = null;

        var isUpdated = await _userService.UpdateAsync(user.Id, invalidUserDto);

        Assert.False(isUpdated);
    }

    [Fact]
    public async Task UpdateUserAsync_UserDoesNotExist_ReturnsFalse()
    {
        var nonExistentUserId = Guid.NewGuid();

        var userDto = new UserCreateDto
        {
            Username = "NonExistentUser",
            Email = "test@example.com",
            Password = "hashedpassword"
        };

        var isUpdated = await _userService.UpdateAsync(nonExistentUserId, userDto);

        Assert.False(isUpdated);
    }

    [Fact]
    public async Task DeleteUserAsync_UserExists_ReturnsTrue()
    {
        var user = _data[0];

        var isDeleted = await _userService.DeleteAsync(user.Id);

        Assert.True(isDeleted);

        var deletedUser = await _userService.GetByIdAsync(user.Id);

        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task DeleteUserAsync_UserDoesNotExist_ReturnsFalse()
    {
        var nonExistentUserId = Guid.NewGuid();

        var isDeleted = await _userService.DeleteAsync(nonExistentUserId);

        Assert.False(isDeleted);
    }
}