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
            Name = "Game",
            Description = "Game Description",
            PublisherId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid()
        };
        var game = await TestHelper.CreateRecordAsync<GameDto>(Client, "/games", newGame);
        var gameId = game.Id;

        var request = new HttpRequestMessage(HttpMethod.Get, $"/games/{gameId}");
        var response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var fetchedDevice = await response.Content.ReadFromJsonAsync<GameDto>();
        Assert.NotNull(fetchedDevice);
        Assert.Equal("TP-Link", fetchedDevice.Name);
    }

    [Fact]
    public async Task GetGameById_ShouldReturnNotFound_WhenGameDoesNotExist()
    {
        var newId = Guid.NewGuid().ToString();
        var request = new HttpRequestMessage(HttpMethod.Get, "/games/" + newId);
        var response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}