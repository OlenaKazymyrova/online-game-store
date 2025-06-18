using AutoMapper;
using OnlineGameStore.BLL.DTOs.Games;
using OnlineGameStore.BLL.Mapping.Resolvers;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Profiles;

public class BllGameMappingProfile : Profile
{
    public BllGameMappingProfile()
    {
        CreateMap<Game, GameDto>()
            .ForMember(dest => dest.PlatformsIds, opt => opt.MapFrom(src => src.Platforms.Select(p => p.Id)))
            .ForMember(dest => dest.GenresIds, opt => opt.MapFrom(src => src.Genres.Select(g => g.Id)));

        CreateMap<GameDto, Game>()
            .ForMember(dest => dest.Platforms, opt => opt.MapFrom<GameResolver>())
            .ForMember(dest => dest.Genres, opt => opt.MapFrom<GameResolver>());

        CreateMap<GameCreateDto, Game>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Platforms, opt => opt.MapFrom<GameResolver>())
            .ForMember(dest => dest.Genres, opt => opt.MapFrom<GameResolver>());
    }
}