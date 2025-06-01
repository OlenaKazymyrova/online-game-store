using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.UI.Tests.DataGenerators;
using System.Net;
using System.Net.Http.Json;

namespace OnlineGameStore.UI.Tests.Tests;

public class GenreControllerTests(ControllerTestsHelper factory) : BaseControllerTests(factory)
{
    private const int _dtoAmountToGenerate = 1;

    private static GenreDto GenGenreDto()
    {
        var genreGen = new GenreDtoDataGenerator();
        return genreGen.Generate(_dtoAmountToGenerate).First();
    }

    [Fact]
    public async Task Create_GenreNotExist_ReturnsLocationUri()
    {
        var newGenreDto = GenGenreDto();
        var postResponse = await Client.PostAsJsonAsync("/genres", newGenreDto);

        postResponse.EnsureSuccessStatusCode();

        var createdGenre = await postResponse.Content.ReadFromJsonAsync<GenreDto>();

        var location = postResponse.Headers.Location;

        Assert.NotNull(createdGenre);
        Assert.NotNull(location);
        Assert.NotNull(createdGenre);
        Assert.EndsWith($"/Genres/{createdGenre.Id}", location.ToString());
        Assert.Equal(newGenreDto, createdGenre);
    }

    [Fact]
    public async Task Create_GenreAlreadyExists_ReturnsBadRequest()
    {
        var newGenreDto = GenGenreDto();
        var postResponse1 = await Client.PostAsJsonAsync("/genres", newGenreDto);

        postResponse1.EnsureSuccessStatusCode();

        var postReponse2 = await Client.PostAsJsonAsync("/genres", newGenreDto);

        Assert.Equal(HttpStatusCode.Conflict, postReponse2.StatusCode);
    }

    [Fact]
    public async Task GetGenre_GenreExists_ReturnsGenre()
    {
        var newGenreDto = GenGenreDto();

        var postResponse = await Client.PostAsJsonAsync("/genres", newGenreDto);
        postResponse.EnsureSuccessStatusCode();

        var createdGenre = await postResponse.Content.ReadFromJsonAsync<GenreDto>();
        Assert.NotNull(createdGenre);

        var getResponse = await Client.GetAsync($"/genres/{createdGenre!.Id}");
        getResponse.EnsureSuccessStatusCode();

        var fetchedGenre = await getResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.NotNull(fetchedGenre);
        Assert.Equal(createdGenre, fetchedGenre);
    }

    [Fact]
    public async Task GetGenre_GenreNotExists_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var getResponse = await Client.GetAsync($"/genres/{id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}