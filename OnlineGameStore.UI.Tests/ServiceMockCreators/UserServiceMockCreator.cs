using Moq;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Exceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class UserServiceMockCreator :
    ServiceMockCreator<User, UserCreateDto, UserReadDto, UserCreateDto, UserReadDto, IUserService>
{
    public UserServiceMockCreator(List<User> data) :
        base(data)
    {
    }

    protected override void SetupAdd(Mock<IUserService> mock)
    {
        mock.Setup(service => service.AddAsync(It.IsAny<UserCreateDto>()))
            .ReturnsAsync((UserCreateDto userCreateDto) =>
            {
                var user = _mapper.Map<User>(userCreateDto);

                if (_data.Any(u => u.Email == user.Email))
                    throw new ConflictException("Email exists");

                if (_data.Any(u => u.Username == user.Username))
                    throw new ConflictException("Username exists");

                _data.Add(user);

                return new UserReadDto
                {
                    Username = user.Username,
                    Email = user.Email
                };
            });
    }
}