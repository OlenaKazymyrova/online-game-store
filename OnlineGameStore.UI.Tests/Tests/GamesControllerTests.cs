using System.Net;
using System.Net.Http.Json;
using OnlineGameStore.BLL.DTOs;

namespace OnlineGameStore.UI.Tests.Tests;

public class GamesControllerTests(ControllerTestsHelper helper) : BaseControllerTests(helper)
{
    [Fact]
    public async Task GetByIdAsync_GameValid_ReturnsGame()
    {
        var newGame = new GameDto
        {
            Id = Guid.NewGuid(),
            Name = "Game",
            Description = "Game Description",
            PublisherId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid()
        };

        var postRequest = await Client.PostAsJsonAsync("/games", newGame);
        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var game = await postRequest.Content.ReadFromJsonAsync<GameDto>();
        var gameId = game!.Id;

        var getRequest = await Client.GetAsync($"games/{gameId}");
        Assert.Equal(HttpStatusCode.OK, getRequest.StatusCode);

        var fetchedGame = await getRequest.Content.ReadFromJsonAsync<GameDto>();
        Assert.NotNull(fetchedGame);
        Assert.Equal(gameId, fetchedGame.Id);
    }

    [Fact]
    public async Task GetByIdAsync_GameDoesNotExist_ReturnsNotFound()
    {
        var newId = Guid.NewGuid().ToString();
        var getRequest = await Client.GetAsync($"games/{newId}");

        Assert.Equal(HttpStatusCode.NotFound, getRequest.StatusCode);
    }

    [Fact]
    public async Task GetAllAsync_GamesExist_ReturnsGamesList()
    {
        var getRequest = await Client.GetAsync("/games");

        Assert.Equal(HttpStatusCode.OK, getRequest.StatusCode);

        var games = await getRequest.Content.ReadFromJsonAsync<IEnumerable<GameDto>>();

        Assert.NotNull(games);
        Assert.NotEmpty(games);
    }

    [Fact]
    public async Task CreateAsync_GameIsValid_ReturnsCreatedGame()
    {
        var newGame = new GameDto
        {
            Id = Guid.NewGuid(),
            Name = "New Game",
            Description = "Action",
            PublisherId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid()
        };

        var postRequest = await Client.PostAsJsonAsync("/games", newGame);

        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var createdGame = await postRequest.Content.ReadFromJsonAsync<GameDto>();

        Assert.NotNull(createdGame);
        Assert.Equal(newGame.Id, createdGame.Id);
    }

    [Fact]
    public async Task CreateAsync_GameNotValid_ReturnsError()
    {
        var notAGame = new List<GameDto>();

        var postRequest = await Client.PostAsJsonAsync("/games", notAGame);

        Assert.Equal(HttpStatusCode.BadRequest, postRequest.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_GameExists_ReturnsNoContent()
    {
        var newGame = new GameDto
        {
            Id = Guid.NewGuid(),
            Name = "Game to Delete",
            Description = "Game Description",
            PublisherId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid()
        };

        var postRequest = await Client.PostAsJsonAsync("/games", newGame);
        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var game = await postRequest.Content.ReadFromJsonAsync<GameDto>();
        var gameId = game!.Id;

        var deleteRequest = await Client.DeleteAsync($"games/{gameId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteRequest.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_GameDoesNotExist_ReturnsNotFound()
    {
        var newId = Guid.NewGuid().ToString();
        var deleteRequest = await Client.DeleteAsync($"games/{newId}");

        Assert.Equal(HttpStatusCode.NotFound, deleteRequest.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_GameAlreadyDeleted_ReturnsNotFound()
    {
        var newGame = new GameDto
        {
            Id = Guid.NewGuid(),
            Name = "Game to Delete",
            Description = "Game Description",
            PublisherId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid()
        };

        var postRequest = await Client.PostAsJsonAsync("/games", newGame);
        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var game = await postRequest.Content.ReadFromJsonAsync<GameDto>();
        var gameId = game!.Id;

        _ = await Client.DeleteAsync($"games/{gameId}"); // First delete request
        var secondDeleteRequest = await Client.DeleteAsync($"games/{gameId}");
        Assert.Equal(HttpStatusCode.NotFound, secondDeleteRequest.StatusCode);
    }
}