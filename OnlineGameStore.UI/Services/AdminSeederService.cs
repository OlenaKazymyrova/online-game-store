using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.UI.Services;

public class AdminSeederService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public AdminSeederService(IServiceProvider serviceProvider)

    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OnlineGameStoreDbContext>();

        if (dbContext == null)
            throw new ArgumentNullException(nameof(dbContext));

        if (await dbContext.Users.AnyAsync(cancellationToken))
            return;

        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();

        var adminUsername = configuration["AdminSeed:Username"];
        var adminEmail = configuration["AdminSeed:Email"];
        var adminPassword = configuration["AdminSeed:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword) ||
            string.IsNullOrWhiteSpace(adminUsername))
        {
            Console.WriteLine("Admin seed data missing from configuration.");
            return;
        }

        var adminRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Admin", cancellationToken);

        if (adminRole == null) // In case our Role seeding fails or starts later
        {
            Console.WriteLine("Admin role not found in the database.");

            var newAdminRole = new Role
            {
                Name = "Admin",
                Description = "Administrator role with full permissions"
            };
            try
            {
                dbContext.Roles.Add(newAdminRole);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create admin role: {ex.Message}");
                return;
            }

            adminRole = newAdminRole;
        }

        var hasher = new PasswordHasher<User>();

        var adminUser = new User
        {
            Email = adminEmail,
            UserName = adminUsername
        };

        adminUser.PasswordHash = hasher.HashPassword(adminUser, adminPassword);

        try
        {
            dbContext.Users.Add(adminUser);
            dbContext.UserRoles.Add(new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to seed admin user: {ex.Message}");
            return;
        }

        Console.WriteLine($"Seeded default admin user: {adminEmail}");
    }
}