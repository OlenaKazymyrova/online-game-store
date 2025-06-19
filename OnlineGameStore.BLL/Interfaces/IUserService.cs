using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IUserService : IService<User, UserCreateDto, UserReadDto, UserCreateDto>
{
}