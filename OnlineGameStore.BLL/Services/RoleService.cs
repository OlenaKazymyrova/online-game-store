using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class RoleService : Service<Role, RoleCreateDto, RoleReadDto, RoleCreateDto>, IRoleService
{
    public RoleService(IRoleRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}