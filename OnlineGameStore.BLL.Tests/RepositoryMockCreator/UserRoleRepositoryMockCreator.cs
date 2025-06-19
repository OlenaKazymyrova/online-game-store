using Moq;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public class UserRoleRepositoryMockCreator
{
    private readonly List<UserRole> _data;

    public UserRoleRepositoryMockCreator(List<UserRole> data)
    {
        _data = data;
    }

    public IUserRoleRepository Create()
    {
        var mock = new Mock<IUserRoleRepository>();
        SetupGetUserRolesAsync(mock);
        SetupUserHasRoleAsync(mock);
        SetupAddUserRoleAsync(mock);
        SetupRemoveUserRoleAsync(mock);
        return mock.Object;
    }

    private void SetupGetUserRolesAsync(Mock<IUserRoleRepository> mock)
    {
        mock.Setup(x => x.GetUserRolesAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId) =>
                _data.Where(ur => ur.UserId == userId).Select(ur => ur.Role).ToList());
    }

    private void SetupUserHasRoleAsync(Mock<IUserRoleRepository> mock)
    {
        mock.Setup(x => x.UserHasRoleAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId, Guid roleId) =>
                _data.Any(ur => ur.UserId == userId && ur.RoleId == roleId));
    }

    private void SetupAddUserRoleAsync(Mock<IUserRoleRepository> mock)
    {
        mock.Setup(x => x.AddUserRoleAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId, Guid roleId) =>
            {
                if (_data.Any(ur => ur.UserId == userId && ur.RoleId == roleId))
                    return false;

                var userRole = new UserRole { UserId = userId, RoleId = roleId };
                _data.Add(userRole);
                return true;
            });
    }

    private void SetupRemoveUserRoleAsync(Mock<IUserRoleRepository> mock)
    {
        mock.Setup(x => x.RemoveUserRoleAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId, Guid roleId) =>
            {
                var userRole = _data.FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == roleId);
                if (userRole == null)
                    return false;

                _data.Remove(userRole);
                return true;
            });
    }
}