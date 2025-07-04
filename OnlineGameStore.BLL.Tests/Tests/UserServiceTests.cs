using AutoMapper;
using Moq;
using OnlineGameStore.BLL.Authentication.Interface;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Exceptions;
using OnlineGameStore.BLL.Mapping.Profiles;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;

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
        var mockPasswordHasher = new Mock<IPasswordHasher>();
        var mockJwtProvider = new Mock<IJwtProvider>();

        _userService = new UserService(mockRepository, mapper, mockPasswordHasher.Object, mockJwtProvider.Object);
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
    public async Task GetUserAsync_UserDoesNotExist_ThrowsNotFoundException()
    {
        var nonExistentUserId = Guid.NewGuid();

        await Assert.ThrowsAsync<NotFoundException>(async () => await _userService.GetByIdAsync(nonExistentUserId));
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
    public async Task AddUserAsync_InvalidUser_ThrowsValidationException()
    {
        UserCreateDto? invalidUser = null;

        await Assert.ThrowsAsync<ValidationException>(async () => await _userService.AddAsync(invalidUser!));
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
    public async Task UpdateUserAsync_InvalidData_ThrowsValidationException()
    {
        var user = _data[0];

        UserCreateDto? invalidUserDto = null;

        await Assert.ThrowsAsync<ValidationException>(async () =>
            await _userService.UpdateAsync(user.Id, invalidUserDto!));
    }

    [Fact]
    public async Task UpdateUserAsync_UserDoesNotExist_ThrowsNotFoundException()
    {
        var nonExistentUserId = Guid.NewGuid();

        var userDto = new UserCreateDto
        {
            Username = "NonExistentUser",
            Email = "test@example.com",
            Password = "hashedpassword"
        };

        await Assert.ThrowsAsync<NotFoundException>(async () => await _userService.UpdateAsync(nonExistentUserId, userDto));
    }

    [Fact]
    public async Task DeleteUserAsync_UserExists_ThrowsNotFoundException()
    {
        var user = _data[0];

        var isDeleted = await _userService.DeleteAsync(user.Id);

        Assert.True(isDeleted);

        await Assert.ThrowsAsync<NotFoundException>(async () => await _userService.GetByIdAsync(user.Id));
    }

    [Fact]
    public async Task DeleteUserAsync_UserDoesNotExist_ThrowsNotFoundException()
    {
        var nonExistentUserId = Guid.NewGuid();

        await Assert.ThrowsAsync<NotFoundException>(async () => await _userService.DeleteAsync(nonExistentUserId));
    }
}