using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.UI.Services;

public class RoleSeederService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly List<Role> Roles =
    [
        new Role { Name = "Admin", Description = "Administrator with full access" },
        new Role { Name = "User", Description = "Regular user with limited access" },
        new Role { Name = "Guest", Description = "Guest user with minimal access" }
    ];

    public RoleSeederService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OnlineGameStoreDbContext>();

        try
        {
            if (!await dbContext.Roles.AnyAsync(stoppingToken))
            {
                await dbContext.Roles.AddRangeAsync(Roles, stoppingToken);
                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine("Role seeding operation was canceled.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding roles: {ex.Message}");
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }
}