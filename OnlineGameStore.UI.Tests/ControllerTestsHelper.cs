using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.DataGenerators;
using OnlineGameStore.UI.Tests.ServiceMockCreators;

namespace OnlineGameStore.UI.Tests;

public class BaseControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<ControllerTestsHelper>
{
    protected readonly HttpClient Client = factory.CreateClient();
}

public class ControllerTestsHelper : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var gameGen = new GameDtoDataGenerator();
        var gameDtoData = gameGen.Generate(100);
        var gameService = new GameServiceMockCreator(gameDtoData).Create();

        var genreGen = new GenreDtoDataGenerator();
        var genreDtoData = genreGen.Generate(100);
        var genreService = new GenreServiceMockCreator(genreDtoData).Create();

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IGameService>();
            services.AddSingleton(gameService);

            services.RemoveAll<IGenreService>();
            services.AddSingleton(genreService);
        });
    }
}