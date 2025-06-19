using AutoMapper;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _repository;
    private readonly IMapper _mapper;

    public UserRoleService(IUserRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
}