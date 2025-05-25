using System.Net;
using System.Net.Http.Json;
using OnlineGameStore.BLL.DTOs;

namespace OnlineGameStore.UI.Tests;

public class GameControllerTests(ControllerTestsHelper helper) : BaseControllerTests(helper)
{
    [Fact]
    public async Task GetGameById_ShouldReturnGame_WhenExists()
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
    public async Task GetGameById_ShouldReturnNotFound_WhenGameDoesNotExist()
    {
        var newId = Guid.NewGuid().ToString();
        var request = await Client.GetAsync($"games/{newId}");

        Assert.Equal(HttpStatusCode.NotFound, request.StatusCode);
    }

    [Fact]
    public async Task CreateGame_ShouldReturnCreated_WhenGameIsValid()
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

        var request = await Client.PostAsJsonAsync("/games", newGame);
        if (request.StatusCode == HttpStatusCode.InternalServerError)
        {
            var errorContent = await request.Content.ReadAsStringAsync();
            throw new Exception($"Internal Server Error: {errorContent}");
        }

        Assert.Equal(HttpStatusCode.Created, request.StatusCode);

        var createdGame = await request.Content.ReadFromJsonAsync<GameDto>();

        Assert.NotNull(createdGame);
        Assert.Equal(newGame.Id, createdGame.Id);
    }
}