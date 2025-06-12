using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Mapping.Resolvers;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Profiles;

public class BllGenreMappingProfile : Profile
{
    public BllGenreMappingProfile()
    {
        CreateMap<Genre, GenreDto>().ReverseMap();

        CreateMap<Genre, GenreReadDto>()
            .ForMember(dest => dest.GamesIds, opt => opt.MapFrom<GenreResolver>());

        CreateMap<GenreCreateDto, Genre>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.ParentGenre, opt => opt.Ignore())
                .ForMember(dest => dest.Games, opt => opt.MapFrom<GenreResolver>());
    }
}
