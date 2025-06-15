using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.ServiceMockCreators;
using OnlineGameStore.SharedLogic.Pagination;
using OnlineGameStore.UI.Aggregation;

namespace OnlineGameStore.UI.Tests.Tests;

public class GamesControllerTests
{
    private readonly HttpClient _client;

    public GamesControllerTests()
    {
        var data = new GameEntityGenerator().Generate(100);
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
    public async Task Get_WithoutExplicitPagination_GetsJsonWithDefaultPaginationAndListOfGames()
    {
        var defaultPagingParams = new PagingParams();
        var getRequest = await _client.GetAsync("api/Games");

        Assert.Equal(HttpStatusCode.OK, getRequest.StatusCode);

        var gamesPaginatedResponse = await getRequest.Content.ReadFromJsonAsync<PaginatedResponse<GameDto>>();

        // NOTE: the default pageSize must be less than default number of entities to generate
        Assert.NotNull(gamesPaginatedResponse);
        Assert.Equal(gamesPaginatedResponse.Items.Count(), defaultPagingParams.PageSize);
    }

    [Fact]
    public async Task Get_WithValidParameters_ReturnsOkResult()
    {
        var pagingParams = new PagingParams();
        var gameFilters = new GameAggregationParams
        {
            SortBy = "name",
            SortOrder = "asc"
        };

        var game1 = GetGameCreateDto("3 Game A", "Description A", 49.99m, new DateTime(2020, 1, 1));
        var game2 = GetGameCreateDto("2 Game B", "Description B", 59.99m, new DateTime(2021, 1, 1));
        var game3 = GetGameCreateDto("1 Game C", "Description C", 39.99m, new DateTime(2022, 1, 1));

        await _client.PostAsJsonAsync("api/Games", game1);
        await _client.PostAsJsonAsync("api/Games", game2);
        await _client.PostAsJsonAsync("api/Games", game3);

        var getRequest = await _client.GetAsync($"api/Games"
            + $"?pageSize={pagingParams.PageSize}&pageNumber={pagingParams.Page}"
            + $"&sortBy={gameFilters.SortBy}&sortOrder={gameFilters.SortOrder}");

        var gamesPaginatedResponse = await getRequest.Content.ReadFromJsonAsync<PaginatedResponse<GameDto>>();

        Assert.Equal(HttpStatusCode.OK, getRequest.StatusCode);
        Assert.NotNull(gamesPaginatedResponse);
        Assert.NotEmpty(gamesPaginatedResponse.Items);
        Assert.Equal(gamesPaginatedResponse.Items.First().Name, game3.Name);
    }

    [Fact]
    public async Task Get_WithInvalidParameters_ReturnsBadRequest()
    {
        var pagingParams = new PagingParams { PageSize = 10, Page = 1 };
        var gameFilters = new GameAggregationParams
        {
            SortBy = "invalid",
            SortOrder = "asc"
        };

        var getRequest = await _client.GetAsync($"api/Games"
            + $"?pageSize={pagingParams.PageSize}&pageNumber={pagingParams.Page}"
            + $"&sortBy={gameFilters.SortBy}&sortOrder={gameFilters.SortOrder}");

        Assert.Equal(HttpStatusCode.BadRequest, getRequest.StatusCode);
    }

    [Fact]
    public async Task CreateAsync_GameIsValid_ReturnsCreatedGame()
    {
        var newGame = GetGameCreateDto();

        var postRequest = await _client.PostAsJsonAsync("api/Games", newGame);

        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var createdGame = await postRequest.Content.ReadFromJsonAsync<GameDto>();

        Assert.NotNull(createdGame);
        Assert.Equal(newGame.Name, createdGame.Name);
        Assert.Equal(newGame.Description, createdGame.Description);
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

    [Fact]
    public async Task UpdatePutAsync_GameExists_ReturnsUpdatedGame()
    {
        var gameCreateDto = GetGameCreateDto();

        var postRequest = await _client.PostAsJsonAsync("api/Games", gameCreateDto);

        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var createdGame = await postRequest.Content.ReadFromJsonAsync<GameDto>();
        var gameId = createdGame!.Id;

        gameCreateDto.Name = "Updated Game Name";

        var putRequest = await _client.PutAsJsonAsync($"api/Games/{createdGame.Id}", gameCreateDto);

        Assert.Equal(HttpStatusCode.OK, putRequest.StatusCode);

        var getRequest = await _client.GetAsync("api/Games/" + gameId);

        var updatedGame = await getRequest.Content.ReadFromJsonAsync<GameDto>();

        Assert.NotNull(updatedGame);
        Assert.Equal(gameCreateDto.Name, updatedGame.Name);
    }

    [Fact]
    public async Task UpdatePutAsync_GameDoesNotExist_ReturnsNotFound()
    {
        var newGame = GetGameDto();
        newGame.Id = Guid.NewGuid();

        var putRequest = await _client.PutAsJsonAsync($"api/Games/{newGame.Id}", newGame);

        Assert.Equal(HttpStatusCode.NotFound, putRequest.StatusCode);
    }

    [Fact]
    public async Task UpdatePutAsync_GameNotValid_ReturnsBadRequest()
    {
        var notAGame = new List<GameDto>();

        var putRequest = await _client.PutAsJsonAsync($"api/Games/{Guid.NewGuid()}", notAGame);

        Assert.Equal(HttpStatusCode.BadRequest, putRequest.StatusCode);
    }

    [Fact]
    public async Task UpdatePatchAsync_GameExists_UpdatesGame()
    {
        var newGame = GetGameDto();

        var postResponse = await _client.PostAsJsonAsync("api/Games", newGame);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var created = await postResponse.Content.ReadFromJsonAsync<GameDto>();
        var id = created!.Id;

        const string newName = "Updated Game Name";

        var patchDoc = new JsonPatchDocument<GameDto>();
        patchDoc.Replace(g => g.Name, newName);

        var json = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");

        var patchResponse = await _client.PatchAsync($"api/Games/{id}", content);
        Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);

        var getResponse = await _client.GetAsync($"api/Games/{id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<GameDto>();

        Assert.Equal(newName, updated!.Name);
    }

    [Fact]
    public async Task UpdatePatchAsync_GameDoesNotExist_ReturnsNotFound()
    {
        var newGame = GetGameDto();
        newGame.Id = Guid.NewGuid();

        const string newName = "Updated Game Name";

        var patchDoc = new JsonPatchDocument<GameDto>();
        patchDoc.Replace(g => g.Name, newName);

        var json = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");

        var patchRequest = await _client.PatchAsync($"api/Games/{newGame.Id}", content);

        Assert.Equal(HttpStatusCode.NotFound, patchRequest.StatusCode);
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
            LicenseId = Guid.NewGuid(),
            Price = price,
            ReleaseDate = releaseDate
        };
    }

    private GameCreateDto GetGameCreateDto(
        string name = "Test Game",
        string description = "Test Description",
        decimal price = 59.99m,
        DateTime releaseDate = default)
    {
        return new GameCreateDto
        {
            Name = name,
            Description = description,
            PublisherId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid(),
            Price = price,
            ReleaseDate = releaseDate
        };
    }
}