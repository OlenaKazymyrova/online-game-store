using AutoMapper;
using OnlineGameStore.BLL.DTOs.Platforms;
using OnlineGameStore.BLL.Mapping.Resolvers;

namespace OnlineGameStore.BLL.Mapping.Profiles;

public class BllPlatformMappingProfile : Profile
{
    public BllPlatformMappingProfile()
    {
        CreateMap<Platform, PlatformDetailedDto>()
            .ForMember(dest => dest.Games, opt => opt.MapFrom(src => src.Games));

        CreateMap<Platform, PlatformBasicDto>();

        CreateMap<Platform, PlatformDto>()
            .ForMember(dest => dest.GamesIds, opt => opt.MapFrom(src => src.Games.Select(game => game.Id)));

        CreateMap<PlatformCreateDto, Platform>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
            .ForMember(dest => dest.Games, opt => opt.MapFrom<PlatformResolver>());
    }
}