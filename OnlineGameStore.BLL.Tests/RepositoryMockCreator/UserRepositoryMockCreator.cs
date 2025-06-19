using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public class UserRepositoryMockCreator : RepositoryMockCreator<User, IUserRepository>
{
    public UserRepositoryMockCreator(List<User> users) : base(users)
    {
    }
}