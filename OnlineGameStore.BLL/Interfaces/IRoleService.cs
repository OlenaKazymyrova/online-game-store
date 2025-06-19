using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IRoleService : IService<Role, RoleCreateDto, RoleReadDto, RoleCreateDto, RoleReadDto>
{
}