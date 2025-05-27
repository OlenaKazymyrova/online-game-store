using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mappings;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<PlatformDto, Platform>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.GamePlatforms, opt => opt.MapFrom(src => src.GameIds.Select(id => new GamePlatform { GameId = id })));


        CreateMap<Platform, PlatformResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.GameIds, opt => opt.MapFrom(src => src.GamePlatforms.Select(gp => gp.GameId)));

        CreateMap<Platform, PlatformDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.GameIds, opt => opt.MapFrom(src => src.GamePlatforms.Select(gp => gp.GameId)));

    }
}