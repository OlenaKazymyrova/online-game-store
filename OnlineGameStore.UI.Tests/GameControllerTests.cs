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

        var request1 = await Client.PostAsJsonAsync("/games", newGame);
        Assert.Equal(HttpStatusCode.Created, request1.StatusCode);
        var game = await request1.Content.ReadFromJsonAsync<GameDto>();
        var gameId = game!.Id;

        var request2 = new HttpRequestMessage(HttpMethod.Get, $"/games/{gameId}");
        var response = await Client.SendAsync(request2);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var fetchedGame = await response.Content.ReadFromJsonAsync<GameDto>();
        Assert.NotNull(fetchedGame);
        Assert.Equal(gameId, fetchedGame.Id);
    }

    [Fact]
    public async Task GetGameById_ShouldReturnNotFound_WhenGameDoesNotExist()
    {
        var newId = Guid.NewGuid().ToString();
        var request = new HttpRequestMessage(HttpMethod.Get, "/games/" + newId);
        var response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
        var createdGame = await request.Content.ReadFromJsonAsync<GameDto>();

        Assert.NotNull(createdGame);
        Assert.Equal(newGame.Id, createdGame.Id);
    }
}