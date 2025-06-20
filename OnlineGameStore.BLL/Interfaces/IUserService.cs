using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IUserService : IService<User, UserCreateDto, UserReadDto, UserCreateDto, UserReadDto>
{
}