using OnlineGameStore.UI.Mapping;

namespace OnlineGameStore.UI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUiServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UiMappingProfile));

        return services;
    }
}