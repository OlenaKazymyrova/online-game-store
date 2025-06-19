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
    private readonly IMapper _mapper;

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

        var userRepository = new UserRepositoryMockCreator(userData, userRoleRepository).Create();
        var roleRepository = new RoleRepositoryMockCreator(roleData, userRoleRepository).Create();

        _userRoleService = new UserRoleService(userRoleRepository, userRepository, roleRepository, mapper);
    }
}