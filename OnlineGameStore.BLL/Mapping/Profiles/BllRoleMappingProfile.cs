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
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Permissions, opt => opt.Ignore())
            .ForMember(dest => dest.Users, opt => opt.Ignore())
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
            .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());;

        CreateMap<Role, RoleReadDto>();
    }
}