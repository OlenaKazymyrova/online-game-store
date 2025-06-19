using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public class RoleRepositoryMockCreator : RepositoryMockCreator<Role, IRoleRepository>
{
    public RoleRepositoryMockCreator(List<Role> roles)
        : base(roles)
    {
    }
}