using AutoMapper;
using OnlineGameStore.BLL.Mapping.Profiles;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.Tests;

public class RoleServiceTests
{
    private readonly RoleService _roleService;
    private readonly IMapper _mapper;
    private List<Role> _data;

    public RoleServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BllRoleMappingProfile>());
        _mapper = config.CreateMapper();

        var userRoleRepository = SetupUserRoleMock();

        var repoMock = new RoleRepositoryMockCreator(_data!, userRoleRepository);
        var mockRepository = repoMock.Create();

        _roleService = new RoleService(mockRepository, _mapper);
    }

    private IUserRoleRepository SetupUserRoleMock()
    {
        var roleGen = new RoleEntityGenerator();
        var userGen = new UserEntityGenerator();
        var userRoleGen = new UserRoleEntityGenerator();

        var userData = userGen.Generate(100);
        _data = roleGen.Generate(100);

        var userRoleData = userRoleGen.Generate(100, userData, _data);

        var userRoleRepoMock = new UserRoleRepositoryMockCreator(userRoleData);
        var mockUserRoleRepository = userRoleRepoMock.Create();

        return mockUserRoleRepository;
    }
}