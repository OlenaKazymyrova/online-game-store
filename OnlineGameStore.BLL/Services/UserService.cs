using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class UserService : Service<User, UserCreateDto, UserReadDto, UserCreateDto, UserReadDto>, IUserService
{
    public UserService(IUserRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}