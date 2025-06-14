using Microsoft.Extensions.DependencyInjection;
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
        services.AddAutoMapper(typeof(BllGameMappingProfile));
        services.AddAutoMapper(typeof(BllGenreMappingProfile));
        services.AddScoped<GameResolver>();  // no parameterless constructor defined
        services.AddScoped<GenreResolver>();

        return services;
    }
}