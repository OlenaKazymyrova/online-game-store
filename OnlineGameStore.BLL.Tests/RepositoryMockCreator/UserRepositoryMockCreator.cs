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
            .Returns(async (Guid userId) =>
            {
                var user = _data.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return false;

                var roles = await _userRoleRepository.GetUserRolesAsync(userId);
                foreach (var role in roles)
                {
                    await _userRoleRepository.RemoveUserRoleAsync(user.Id, role.Id);
                }

                _data.Remove(user);
                return true;
            });
    }
}