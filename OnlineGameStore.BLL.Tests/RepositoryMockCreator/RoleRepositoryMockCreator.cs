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
            .Returns(async (Guid roleId) =>
            {
                var role = _data.FirstOrDefault(r => r.Id == roleId);
                if (role == null)
                    return false;

                var users = await _userRoleRepository.GetUsersByRoleAsync(roleId);
                foreach (var user in users)
                {
                    await _userRoleRepository.RemoveUserRoleAsync(user.Id, role.Id);
                }

                _data.Remove(role);
                return true;
            });
    }
}