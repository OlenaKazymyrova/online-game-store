using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.UI.Tests.DataGenerators;
using System.Net;
using System.Net.Http.Json;

namespace OnlineGameStore.UI.Tests.Tests;

public class GenresControllerTests(ControllerTestsHelper factory) : BaseControllerTests(factory)
{
    private const int _dtoAmountToGenerate = 1;

    private static GenreDto GenGenreDto(int count = _dtoAmountToGenerate)
    {
        var genreGen = new GenreDtoDataGenerator();
        return genreGen.Generate(_dtoAmountToGenerate).First();
    }

    [Fact]
    public async Task Create_GenreNotExist_ReturnsLocationUri()
    {
        var newGenreDto = GenGenreDto();
        var postResponse = await Client.PostAsJsonAsync("api/genres", newGenreDto);

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
        var postResponse1 = await Client.PostAsJsonAsync("api/genres", newGenreDto);

        postResponse1.EnsureSuccessStatusCode();

        var postReponse2 = await Client.PostAsJsonAsync("api/genres", newGenreDto);

        Assert.Equal(HttpStatusCode.Conflict, postReponse2.StatusCode);
    }

    [Fact]
    public async Task GetGenre_GenreExists_ReturnsGenre()
    {
        var newGenreDto = GenGenreDto();
        var postResponse = await Client.PostAsJsonAsync("api/genres", newGenreDto);

        postResponse.EnsureSuccessStatusCode();

        var createdGenre = await postResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.NotNull(createdGenre);

        var getResponse = await Client.GetAsync($"api/genres/{createdGenre!.Id}");

        getResponse.EnsureSuccessStatusCode();

        var fetchedGenre = await getResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.NotNull(fetchedGenre);
        Assert.Equal(createdGenre, fetchedGenre);
    }

    [Fact]
    public async Task GetGenre_GenreNotExists_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var getResponse = await Client.GetAsync($"api/genres/{id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_GenreExists_ReturnsNoContent()
    {
        var newGenreDto = GenGenreDto();
        var postResponse = await Client.PostAsJsonAsync("api/genres", newGenreDto);

        postResponse.EnsureSuccessStatusCode();

        var createdGenre = await postResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.NotNull(createdGenre);

        var deleteResponse = await Client.DeleteAsync($"api/genres/{createdGenre!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_GenreNotExists_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var deleteResponse = await Client.DeleteAsync($"api/genres/{id}");

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

        var postResponse1 = await Client.PostAsJsonAsync("api/genres", parentGenre);
        var postResponse2 = await Client.PostAsJsonAsync("api/genres", childGenre);

        postResponse1.EnsureSuccessStatusCode();
        postResponse2.EnsureSuccessStatusCode();

        var deleteResponse = await Client.DeleteAsync($"api/genres/{parentGenre.Id}");

        deleteResponse.EnsureSuccessStatusCode();

        var getChildResponse = await Client.GetAsync($"api/genres/{childGenre.Id}");

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

        var postResponse1 = await Client.PostAsJsonAsync("api/genres", parentGenre);
        var postResponse2 = await Client.PostAsJsonAsync("api/genres", childGenre);

        postResponse1.EnsureSuccessStatusCode();
        postResponse2.EnsureSuccessStatusCode();

        var deleteResponse = await Client.DeleteAsync($"api/genres/{childGenre.Id}");

        deleteResponse.EnsureSuccessStatusCode();

        var getParentResponse = await Client.GetAsync($"api/genres/{parentGenre.Id}");

        getParentResponse.EnsureSuccessStatusCode();

        var fetchedParentGenre = await getParentResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.Equal(fetchedParentGenre, parentGenre);
    }
}