using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.BLL.Authentication.Services;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Enums;
using OnlineGameStore.SharedLogic.Settings;

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
        var passwordHasher = _serviceProvider.GetRequiredService<PasswordHasher>();
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OnlineGameStoreDbContext>();

        if (dbContext == null)
            throw new ArgumentNullException(nameof(dbContext));

        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        await SeedUserAsync(dbContext, configuration, RoleEnum.Admin, "AdminSeed", passwordHasher, cancellationToken);
        await SeedUserAsync(dbContext, configuration, RoleEnum.User, "UserSeed", passwordHasher, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        var usersCount = await dbContext.Users.CountAsync();
        Console.WriteLine($"Users in DB after seeding: {usersCount}");

    }

    private async Task SeedUserAsync(OnlineGameStoreDbContext dbContext, IConfiguration configuration,
        RoleEnum roleEnum, string section, PasswordHasher passwordHasher, CancellationToken cancellationToken)
    {

        var username = configuration[$"{section}:Username"];
        var email = configuration[$"{section}:Email"];
        var password = configuration[$"{section}:Password"];

        if (await dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken))
            return;

        var hashedPassword = passwordHasher.Generate(password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = hashedPassword
        };
        dbContext.Users.Add(user);

        var roleId = SystemPermissionSettings.GetRoleGuid(roleEnum);
        dbContext.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = roleId
        });

    }
}