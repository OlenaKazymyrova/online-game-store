using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Profiles;

public class BllUserMappingProfile : Profile
{
    public BllUserMappingProfile()
    {
        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));

        CreateMap<User, UserReadDto>();
    }
}