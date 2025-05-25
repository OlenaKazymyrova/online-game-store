using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Repositories;

namespace OnlineGameStore.DAL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OnlineGameStoreDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnString"))); // Replace with the actual one

        services.AddScoped<IGameRepository, GameRepository>();
       
        //add other interfaces of repositories

      



        return services;
    }
}