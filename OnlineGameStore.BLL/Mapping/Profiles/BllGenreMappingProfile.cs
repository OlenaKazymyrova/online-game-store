using AutoMapper;
using Microsoft.EntityFrameworkCore.Design.Internal;
using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.BLL.Mapping.Converters;
using OnlineGameStore.BLL.Mapping.Resolvers;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Profiles;

public class BllGenreMappingProfile : Profile
{
    public BllGenreMappingProfile()
    {
        CreateMap<Genre, GenreBasicDto>();

        CreateMap<Genre, GenreDetailedDto>()
            .ForMember(dest => dest.Games, opt => opt.MapFrom(src => src.Games));

        CreateMap<Genre, GenreDto>()
            .ForMember(dest => dest.GamesIds, opt => opt.MapFrom(src => src.Games.Select(game => game.Id)));

        CreateMap<Genre, GenreReadDto>()
            .ForMember(dest => dest.GamesIds, opt => opt.MapFrom(src => src.Games.Select(game => game.Id)));

        CreateMap<GenreCreateDto, Genre>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.ParentGenre, opt => opt.Ignore())
                .ForMember(dest => dest.Games, opt => opt.MapFrom<GenreResolver>());

        CreateMap<Guid, Genre>()
            .ConvertUsing<GuidToGenreConverter>();
    }
}
