using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.DBContext;

public class OnlineGameStoreDbContext(DbContextOptions<OnlineGameStoreDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}