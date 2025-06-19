using Moq;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public class RoleRepositoryMockCreator : RepositoryMockCreator<Role, IRoleRepository>
{
    private readonly IUserRoleRepository _userRoleRepository;

    public RoleRepositoryMockCreator(List<Role> roles, IUserRoleRepository userRoleRepository)
        : base(roles)
    {
        _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
    }
}