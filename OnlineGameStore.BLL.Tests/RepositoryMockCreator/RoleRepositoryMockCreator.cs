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

    protected override void SetupDelete(Mock<IRoleRepository> mock)
    {
        mock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid roleId) =>
            {
                var role = _data.FirstOrDefault(r => r.Id == roleId);
                if (role == null)
                    return false;

                var users = _userRoleRepository.GetUsersByRoleAsync(roleId).Result;
                foreach (var user in users)
                {
                    _userRoleRepository.RemoveUserRoleAsync(user.Id, role.Id).Wait();
                }

                _data.Remove(role);
                return true;
            });
    }
}