using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.DataGenerators;
using OnlineGameStore.UI.Tests.ServiceMockCreators;
using System.Net;
using System.Net.Http.Json;

namespace OnlineGameStore.UI.Tests.Tests;

public class GenresControllerTests
{
    private const int _dtoAmountToGenerate = 1;
    private readonly HttpClient _client;

    public GenresControllerTests()
    {
        var data = new GenreDtoGenerator().Generate(100);
        var mockCreator = new GenreServiceMockCreator(data);
        var factory = new ControllerTestsHelper<IGenreService>(mockCreator);
        _client = factory.CreateClient();
    }


    private static GenreDto GenGenreDto(int count = _dtoAmountToGenerate)
    {
        var genreGen = new GenreDtoGenerator();
        return genreGen.Generate(_dtoAmountToGenerate).First();
    }

    [Fact]
    public async Task Create_GenreNotExist_ReturnsLocationUri()
    {
        var newGenreDto = GenGenreDto();
        var postResponse = await _client.PostAsJsonAsync("api/genres", newGenreDto);

        postResponse.EnsureSuccessStatusCode();

        var createdGenre = await postResponse.Content.ReadFromJsonAsync<GenreDto>();

        var location = postResponse.Headers.Location;

        Assert.NotNull(createdGenre);
        Assert.NotNull(location);
        Assert.NotNull(createdGenre);
        Assert.EndsWith($"api/Genres/{createdGenre.Id}", location.ToString());
        Assert.Equal(newGenreDto, createdGenre);
    }

    [Fact]
    public async Task Create_GenreAlreadyExists_ReturnsConflict()
    {
        var newGenreDto = GenGenreDto();
        var postResponse1 = await _client.PostAsJsonAsync("api/genres", newGenreDto);

        postResponse1.EnsureSuccessStatusCode();

        var postReponse2 = await _client.PostAsJsonAsync("api/genres", newGenreDto);

        Assert.Equal(HttpStatusCode.Conflict, postReponse2.StatusCode);
    }

    [Fact]
    public async Task GetGenre_GenreExists_ReturnsGenre()
    {
        var newGenreDto = GenGenreDto();
        var postResponse = await _client.PostAsJsonAsync("api/genres", newGenreDto);

        postResponse.EnsureSuccessStatusCode();

        var createdGenre = await postResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.NotNull(createdGenre);

        var getResponse = await _client.GetAsync($"api/genres/{createdGenre!.Id}");

        getResponse.EnsureSuccessStatusCode();

        var fetchedGenre = await getResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.NotNull(fetchedGenre);
        Assert.Equal(createdGenre, fetchedGenre);
    }

    [Fact]
    public async Task GetGenre_GenreNotExists_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var getResponse = await _client.GetAsync($"api/genres/{id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_GenreExists_ReturnsNoContent()
    {
        var newGenreDto = GenGenreDto();
        var postResponse = await _client.PostAsJsonAsync("api/genres", newGenreDto);

        postResponse.EnsureSuccessStatusCode();

        var createdGenre = await postResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.NotNull(createdGenre);

        var deleteResponse = await _client.DeleteAsync($"api/genres/{createdGenre!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_GenreNotExists_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var deleteResponse = await _client.DeleteAsync($"api/genres/{id}");

        Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_DeleteParent_SetsChildReferenceToNull()
    {
        var parentGenre = GenGenreDto();
        var childGenre = new GenreDto
        {
            Id = Guid.NewGuid(),
            Name = "Child Genre",
            Description = "Child Description",
            ParentId = parentGenre.Id
        };

        var postParentResponse = await _client.PostAsJsonAsync("api/genres", parentGenre);
        var postChildResponse = await _client.PostAsJsonAsync("api/genres", childGenre);

        postParentResponse.EnsureSuccessStatusCode();
        postChildResponse.EnsureSuccessStatusCode();

        var deleteParentResponse = await _client.DeleteAsync($"api/genres/{parentGenre.Id}");

        deleteParentResponse.EnsureSuccessStatusCode();

        var getChildResponse = await _client.GetAsync($"api/genres/{childGenre.Id}");

        getChildResponse.EnsureSuccessStatusCode();

        var fetchedChildGenre = await getChildResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.NotNull(fetchedChildGenre);
        Assert.Null(fetchedChildGenre.ParentId);
    }

    [Fact]
    public async Task Delete_DeleteChild_ParentNotChanged()
    {
        var parentGenre = GenGenreDto();
        var childGenre = new GenreDto
        {
            Id = Guid.NewGuid(),
            Name = "Child Genre",
            Description = "Child Description",
            ParentId = parentGenre.Id
        };

        var parentResponse = await _client.PostAsJsonAsync("api/genres", parentGenre);
        var childResponse = await _client.PostAsJsonAsync("api/genres", childGenre);

        parentResponse.EnsureSuccessStatusCode();
        childResponse.EnsureSuccessStatusCode();

        var deleteChildResponse = await _client.DeleteAsync($"api/genres/{childGenre.Id}");

        deleteChildResponse.EnsureSuccessStatusCode();

        var getParentResponse = await _client.GetAsync($"api/genres/{parentGenre.Id}");

        getParentResponse.EnsureSuccessStatusCode();

        var fetchedParentGenre = await getParentResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.Equal(fetchedParentGenre, parentGenre);
    }
}