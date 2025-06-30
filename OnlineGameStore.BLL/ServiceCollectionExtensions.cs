using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OnlineGameStore.BLL.Authentication.Interface;
using OnlineGameStore.BLL.Authentication.Services;
using OnlineGameStore.BLL.Authorization;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Mapping.Profiles;
using OnlineGameStore.BLL.Mapping.Resolvers;
using OnlineGameStore.BLL.Services;


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
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddAutoMapper(typeof(BllGameMappingProfile));
        services.AddAutoMapper(typeof(BllGenreMappingProfile));
        services.AddAutoMapper(typeof(BllPlatformMappingProfile));
        services.AddAutoMapper(typeof(BllUserMappingProfile));
        services.AddAutoMapper(typeof(BllRoleMappingProfile));
        services.AddScoped<GameResolver>(); // no parameterless constructor defined
        services.AddScoped<GenreResolver>();
        services.AddScoped<PlatformResolver>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<PasswordHasher>();

        return services;
    }
}