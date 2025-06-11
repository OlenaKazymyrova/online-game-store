using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping;

public class BllMappingProfile : Profile
{
    public BllMappingProfile()
    {
        CreateMap<Game, GameDto>().ReverseMap();

        CreateMap<GameCreateDto, Game>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));

        CreateMap<Genre, GenreDto>().ReverseMap();

        CreateMap<Genre, GenreReadDto>().ReverseMap();

        CreateMap<Platform, PlatformDto>();
        
        CreateMap<PlatformCreateDto, Platform>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
            .ForMember(dest => dest.Games, opt => opt.Ignore());

        CreateMap<GenreCreateDto, Genre>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.ParentGenre, opt => opt.Ignore());

    }
}