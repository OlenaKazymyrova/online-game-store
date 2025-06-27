using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Mapping.Converters;
using OnlineGameStore.BLL.Mapping.Profiles;
using OnlineGameStore.BLL.Mapping.Resolvers;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBllServices(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IPlatformService, PlatformService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserRoleService, UserRoleService>();

        services.AddAutoMapper(typeof(BllGameMappingProfile));
        services.AddAutoMapper(typeof(BllGenreMappingProfile));
        services.AddAutoMapper(typeof(BllPlatformMappingProfile));
        services.AddAutoMapper(typeof(BllUserMappingProfile));
        services.AddAutoMapper(typeof(BllRoleMappingProfile));

        services.AddScoped<GameResolver>();
        services.AddScoped<GenreResolver>();
        services.AddScoped<PlatformResolver>();

        services.AddScoped<ITypeConverter<Guid, Genre>, GuidToGenreConverter>();
        services.AddScoped<ITypeConverter<Guid, Platform>, GuidToPlatformConverter>();

        return services;
    }
}