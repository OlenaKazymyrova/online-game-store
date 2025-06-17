using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Tests.RepositoryCreators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class UserRoleRepositoryTests
{
    private readonly UserRoleRepositoryCreator _creator = new();

    private static UserRole GetUserRole(Guid? userId = null, Guid? roleId = null)
    {
        return new UserRole
        {
            UserId = userId ?? Guid.NewGuid(),
            RoleId = roleId ?? Guid.NewGuid()
        };
    }
}