using AutoMapper;
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
    private readonly IMapper _mapper;
    private List<User> _data;

    public UserServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BllUserMappingProfile>());
        _mapper = config.CreateMapper();

        var userRoleRepository = SetupUserRoleMock();

        var repoMock = new UserRepositoryMockCreator(_data!, userRoleRepository);
        var mockRepository = repoMock.Create();

        _userService = new UserService(mockRepository, _mapper);
    }

    [Fact]
    public async Task GetUserRolesAsync_ShouldReturnUserRoles_WhenUserExists()
    {
        var user = _data[0];

        var expectedUser = await _userService.GetByIdAsync(user.Id);

        Assert.NotNull(expectedUser);
        Assert.Equal(user.UserName, expectedUser.Username);
    }

    private IUserRoleRepository SetupUserRoleMock()
    {
        var roleGen = new RoleEntityGenerator();
        var userGen = new UserEntityGenerator();
        var userRoleGen = new UserRoleEntityGenerator();

        var roleData = roleGen.Generate(100);
        _data = userGen.Generate(100);

        var userRoleData = userRoleGen.Generate(100, _data, roleData);

        var userRoleRepoMock = new UserRoleRepositoryMockCreator(userRoleData);
        var mockUserRoleRepository = userRoleRepoMock.Create();

        return mockUserRoleRepository;
    }
}