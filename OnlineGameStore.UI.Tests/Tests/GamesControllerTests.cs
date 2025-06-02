using System.Net;
using System.Net.Http.Json;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.DataGenerators;
using OnlineGameStore.UI.Tests.ServiceMockCreators;

namespace OnlineGameStore.UI.Tests.Tests;

public class GamesControllerTests
{
    private readonly HttpClient _client;

    public GamesControllerTests()
    {
        var data = new GameDtoDataGenerator().Generate(100);
        var mockCreator = new GameServiceMockCreator(data);
        var factory = new ControllerTestsHelper<IGameService>(mockCreator);
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetByIdAsync_GameValid_ReturnsGame()
    {
        var newGame = GetGameDto();

        var postRequest = await _client.PostAsJsonAsync("api/Games", newGame);

        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var game = await postRequest.Content.ReadFromJsonAsync<GameDto>();
        var gameId = game!.Id;

        var getRequest = await _client.GetAsync($"api/Games/{gameId}");

        Assert.Equal(HttpStatusCode.OK, getRequest.StatusCode);

        var fetchedGame = await getRequest.Content.ReadFromJsonAsync<GameDto>();

        Assert.NotNull(fetchedGame);
        Assert.Equal(gameId, fetchedGame.Id);
    }

    [Fact]
    public async Task GetByIdAsync_GameDoesNotExist_ReturnsNotFound()
    {
        var newId = Guid.NewGuid().ToString();
        var getRequest = await _client.GetAsync($"api/Games/{newId}");

        Assert.Equal(HttpStatusCode.NotFound, getRequest.StatusCode);
    }

    [Fact]
    public async Task GetAllAsync_GamesExist_ReturnsGamesList()
    {
        var getRequest = await _client.GetAsync("api/Games");

        Assert.Equal(HttpStatusCode.OK, getRequest.StatusCode);

        var games = await getRequest.Content.ReadFromJsonAsync<IEnumerable<GameDto>>();

        Assert.NotNull(games);
        Assert.NotEmpty(games);
    }

    [Fact]
    public async Task CreateAsync_GameIsValid_ReturnsCreatedGame()
    {
        var newGame = GetGameDto();

        var postRequest = await _client.PostAsJsonAsync("api/Games", newGame);

        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var createdGame = await postRequest.Content.ReadFromJsonAsync<GameDto>();

        Assert.NotNull(createdGame);
        Assert.Equal(newGame.Id, createdGame.Id);
    }

    [Fact]
    public async Task CreateAsync_PriceIsNegative_ReturnsError()
    {
        var newGame = GetGameDto(price: -1.0m);

        var postRequest = await _client.PostAsJsonAsync("api/Games", newGame);

        Assert.Equal(HttpStatusCode.BadRequest, postRequest.StatusCode);
    }

    [Fact]
    public async Task CreateAsync_GameNotValid_ReturnsError()
    {
        var notAGame = new List<GameDto>();

        var postRequest = await _client.PostAsJsonAsync("api/Games", notAGame);

        Assert.Equal(HttpStatusCode.BadRequest, postRequest.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_GameExists_ReturnsNoContent()
    {
        var newGame = GetGameDto();

        var postRequest = await _client.PostAsJsonAsync("api/Games", newGame);

        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var game = await postRequest.Content.ReadFromJsonAsync<GameDto>();
        var gameId = game!.Id;

        var deleteRequest = await _client.DeleteAsync($"api/Games/{gameId}");

        Assert.Equal(HttpStatusCode.NoContent, deleteRequest.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_GameDoesNotExist_ReturnsNotFound()
    {
        var newId = Guid.NewGuid().ToString();
        var deleteRequest = await _client.DeleteAsync($"api/Games/{newId}");

        Assert.Equal(HttpStatusCode.NotFound, deleteRequest.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_GameAlreadyDeleted_ReturnsNotFound()
    {
        var newGame = GetGameDto();

        var postRequest = await _client.PostAsJsonAsync("api/Games", newGame);

        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var game = await postRequest.Content.ReadFromJsonAsync<GameDto>();
        var gameId = game!.Id;

        var firstDeleteRequest = await _client.DeleteAsync($"api/Games/{gameId}");
        var secondDeleteRequest = await _client.DeleteAsync($"api/Games/{gameId}");

        Assert.Equal(HttpStatusCode.NotFound, secondDeleteRequest.StatusCode);
    }

    private GameDto GetGameDto(
        string name = "Test Game",
        string description = "Test Description",
        decimal price = 59.99m,
        DateTime releaseDate = default)
    {
        return new GameDto
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            PublisherId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid(),
            Price = price,
            ReleaseDate = releaseDate
        };
    }
}