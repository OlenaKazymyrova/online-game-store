using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.DTOs.Roles;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Profiles;

public class BllRoleMappingProfile : Profile
{
    public BllRoleMappingProfile()
    {
        CreateMap<RoleCreateDto, Role>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));

        CreateMap<Role, RoleReadDto>();
    }
}