using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class UserServiceMockCreator :
    ServiceMockCreator<User, UserCreateDto, UserReadDto, UserCreateDto, UserReadDto, IUserService>
{
    public UserServiceMockCreator(List<User> data) :
        base(data)
    {
    }
}