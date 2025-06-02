using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.UI.DTOs.Platform;

namespace OnlineGameStore.UI.Mapping;

public class UiMappingProfile : Profile
{
    public UiMappingProfile()
    {
        CreateMap<PlatformDto, PlatformResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<PlatformRequestDto, PlatformDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.Empty))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

    }
}