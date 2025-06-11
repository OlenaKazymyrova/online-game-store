using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Mapping.Resolvers;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Profiles;

public class BllGameMappingProfile : Profile
{
    public BllGameMappingProfile()
    {
        CreateMap<Game, GameDto>().ReverseMap();

        CreateMap<GameCreateDto, Game>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Platforms, opt => opt.MapFrom<GamePlatformsResolver>())
            .ForMember(dest => dest.Genres, opt => opt.MapFrom<GameGenresResolver>());

    }
}