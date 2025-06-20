using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.DTOs.Roles;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IRoleService : IService<Role, RoleCreateDto, RoleReadDto, RoleCreateDto, RoleReadDto>
{
}