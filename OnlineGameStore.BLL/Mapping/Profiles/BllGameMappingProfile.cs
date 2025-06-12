using AutoMapper;
using Microsoft.Data.SqlClient;
using OnlineGameStore.BLL.DTOs;
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

        CreateMap<GameCreateDto, Game>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Platforms,  opt => opt.MapFrom<GameResolver>())
            .ForMember(dest => dest.Genres, opt => opt.MapFrom<GameResolver>());
    }
}