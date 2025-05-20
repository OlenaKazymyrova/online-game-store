using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL;

public class OnlineGameStoreDbContext(DbContextOptions<OnlineGameStoreDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}