using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineGameStore.DAL;

namespace OnlineGameStore.UI.Tests;

public class BaseControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<ControllerTestsHelper>
{
    protected readonly HttpClient Client = factory.CreateClient();
}

public class ControllerTestsHelper : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContext = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<OnlineGameStoreDbContext>));

            if (dbContext != null)
                services.Remove(dbContext);

            services.AddDbContext<OnlineGameStoreDbContext>(options => { options.UseInMemoryDatabase("TestDb"); });
        });
    }
}