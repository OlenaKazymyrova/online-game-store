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

    protected override void SetupDelete(Mock<IUserRepository> mock)
    {
        mock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId) =>
            {
                var user = _data.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return false;

                var roles = _userRoleRepository.GetUserRolesAsync(userId).Result;
                foreach (var role in roles)
                {
                    _userRoleRepository.RemoveUserRoleAsync(user.Id, role.Id).Wait();
                }

                _data.Remove(user);
                return true;
            });
    }
}