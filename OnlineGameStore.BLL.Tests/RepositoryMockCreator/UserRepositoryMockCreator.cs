using Moq;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public class UserRepositoryMockCreator : RepositoryMockCreator<User, IUserRepository>
{
    private readonly IUserRoleRepository _userRoleRepository;

    public UserRepositoryMockCreator(List<User> users, IUserRoleRepository userRoleRepository)
        : base(users)
    {
        _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
    }
}