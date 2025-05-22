using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Game, GameDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.Publisher))
            .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.Genre))
            .ForMember(dest => dest.LicenseId, opt => opt.MapFrom(src => src.License));
    }
}