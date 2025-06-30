using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Profiles;

public class BllUserMappingProfile : Profile
{
    public BllUserMappingProfile()
    {
        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.PasswordHash,
                opt =>
                    opt.MapFrom(src => Encoding.UTF8.GetString(SHA256.HashData(Encoding.UTF8.GetBytes(src.Password)))))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokenExpiryTime, opt => opt.Ignore())
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

        CreateMap<User, UserReadDto>();
    }
}