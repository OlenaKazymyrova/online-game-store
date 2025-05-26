using Microsoft.Extensions.DependencyInjection;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Mapping;
using OnlineGameStore.BLL.Services;

namespace OnlineGameStore.BLL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBllServices(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}