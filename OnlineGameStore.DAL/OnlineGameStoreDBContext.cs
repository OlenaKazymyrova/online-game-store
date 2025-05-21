using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL;

public class OnlineGameStoreDbContext(DbContextOptions<OnlineGameStoreDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<GamePlatform> GamePlatforms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}