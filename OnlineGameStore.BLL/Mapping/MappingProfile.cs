using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping;

public class BllMappingProfile : Profile
{
    public BllMappingProfile()
    {
        CreateMap<Game, GameDto>()
            .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.Publisher))
            .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.Genre))
            .ForMember(dest => dest.LicenseId, opt => opt.MapFrom(src => src.License))
            .ReverseMap()
            .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.PublisherId))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.GenreId))
            .ForMember(dest => dest.License, opt => opt.MapFrom(src => src.LicenseId));
    }
}