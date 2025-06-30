using OnlineGameStore.DAL.Entities;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.DataGenerators;
using OnlineGameStore.UI.Tests.ServiceMockCreators;
using System.Net;
using System.Net.Http.Json;
using OnlineGameStore.SharedLogic.Pagination;
using OnlineGameStore.BLL.DTOs.Genres;

namespace OnlineGameStore.UI.Tests.Tests;

public class GenresControllerTests
{
    private const int _dtoAmountToGenerate = 100;
    private readonly HttpClient _client;

    public GenresControllerTests()
    {
        var data = new GenreEntityGenerator().Generate(_dtoAmountToGenerate);
        var mockCreator = new GenreServiceMockCreator(data);
        var factory = new ControllerTestsHelper<IGenreService>(mockCreator);
        _client = factory.CreateClient();
    }

    private static Genre GenGenreEntity()
    {
        var genreGen = new GenreEntityGenerator();
        return genreGen.Generate(1).First();
    }

    private static GenreCreateDto GenGenreCreateDto()
    {
        var genreGen = new GenreCreateDtoGenerator();
        return genreGen.Generate(1).First();
    }

    [Fact]
    public async Task Create_GenreNotExist_ReturnsLocationUri()
    {
        var genreCreateDto = GenGenreCreateDto();
        var postResponse = await _client.PostAsJsonAsync("api/genres", genreCreateDto);

        postResponse.EnsureSuccessStatusCode();

        var createdGenre = await postResponse.Content.ReadFromJsonAsync<GenreDto>();

        var location = postResponse.Headers.Location;

        Assert.NotNull(createdGenre);
        Assert.NotNull(location);
        Assert.EndsWith($"api/genres/{createdGenre.Id}", location.ToString());
        Assert.Equal(genreCreateDto.Name, createdGenre.Name);
        Assert.Equal(genreCreateDto.Description, createdGenre.Description);
    }

    // NOTE: the test is to be skipped as the condition for adding a confliciting genre has changed
    //[Fact]
    //public async Task Create_GenreAlreadyExists_ReturnsConflict()
    //{
    //    var newGenreDto = GenGenreDto();
    //    var postResponse1 = await _client.PostAsJsonAsync("api/genres", newGenreDto);

    //    postResponse1.EnsureSuccessStatusCode();

    //    var postReponse2 = await _client.PostAsJsonAsync("api/genres", newGenreDto);

    //    Assert.Equal(HttpStatusCode.Conflict, postReponse2.StatusCode);
    //}

    [Fact]
    public async Task UpdatePut_GenreIsPresent_Success()
    {
        var genreCreateDto = GenGenreCreateDto();

        var postResponse = await _client.PostAsJsonAsync("api/genres", genreCreateDto);

        postResponse.EnsureSuccessStatusCode();

        var createdGenre = await postResponse.Content.ReadFromJsonAsync<GenreReadDto>();

        genreCreateDto.Name = "Awesome updated name";

        var putResponse = await _client.PutAsJsonAsync($"api/genres/{createdGenre!.Id}", genreCreateDto);

        putResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"api/genres/{createdGenre.Id}");

        getResponse.EnsureSuccessStatusCode();

        var updatedGenre = await getResponse.Content.ReadFromJsonAsync<GenreDto>();

        Assert.NotNull(updatedGenre);
        Assert.Equal(updatedGenre.Id, createdGenre.Id);
        Assert.NotEqual(updatedGenre.Name, createdGenre.Name);
        Assert.Equal(updatedGenre.Description, createdGenre.Description);
    }

    [Fact]
    public async Task UpdatePut_GenreIsNotPresent_Failure()
    {
        var newGenreDto = GenGenreCreateDto();

        var putResponse = await _client.PutAsJsonAsync($"api/genres/{Guid.NewGuid()}", newGenreDto);

        Assert.Equal(HttpStatusCode.NotFound, putResponse.StatusCode);
    }

    [Fact]
    public async Task GetGenre_GenreExists_ReturnsGenre()
    {
        var newGenreDto = GenGenreCreateDto();
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
        var newGenreDto = GenGenreCreateDto();
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
    public async Task Delete_DeleteParent_DeletesChild()
    {
        var parentGenre = GenGenreCreateDto();

        var postParentResponse = await _client.PostAsJsonAsync("api/genres", parentGenre);

        postParentResponse.EnsureSuccessStatusCode();

        var createdParent = await postParentResponse.Content.ReadFromJsonAsync<GenreReadDto>();

        Assert.NotNull(createdParent);

        var childGenre = new GenreCreateDto
        {
            Name = "Child Genre",
            Description = "Child Description",
            ParentId = createdParent.Id
        };

        var postChildResponse = await _client.PostAsJsonAsync("api/genres", childGenre);

        postChildResponse.EnsureSuccessStatusCode();

        var createdChild = await postChildResponse.Content.ReadFromJsonAsync<GenreReadDto>();

        Assert.NotNull(createdChild);

        var deleteParentResponse = await _client.DeleteAsync($"api/genres/{createdParent.Id}");

        deleteParentResponse.EnsureSuccessStatusCode();

        var getChildResponse = await _client.GetAsync($"api/genres/{createdChild!.Id}");

        Assert.Equal(HttpStatusCode.NotFound, getChildResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_DeleteChild_ParentNotChanged()
    {
        var parentGenre = GenGenreCreateDto();

        var parentResponse = await _client.PostAsJsonAsync("api/genres", parentGenre);

        parentResponse.EnsureSuccessStatusCode();

        var createdParent = await parentResponse.Content.ReadFromJsonAsync<GenreReadDto>();

        Assert.NotNull(createdParent);

        var childGenre = new GenreCreateDto
        {
            Name = "Child Genre",
            Description = "Child Description",
            ParentId = createdParent.Id
        };

        var childResponse = await _client.PostAsJsonAsync("api/genres", childGenre);

        childResponse.EnsureSuccessStatusCode();

        var createdChild = await childResponse.Content.ReadFromJsonAsync<GenreReadDto>();

        var deleteChildResponse = await _client.DeleteAsync($"api/genres/{createdChild!.Id}");

        deleteChildResponse.EnsureSuccessStatusCode();

        var getParentResponse = await _client.GetAsync($"api/genres/{createdParent.Id}");

        getParentResponse.EnsureSuccessStatusCode();

        var fetchedParentGenre = await getParentResponse.Content.ReadFromJsonAsync<GenreReadDto>();

        Assert.NotNull(fetchedParentGenre);
        Assert.Equal(fetchedParentGenre.Id, createdParent.Id);
    }

    [Fact]
    public async Task Get_WithoutExplicitPagination_GetsJsonWithDefaultPaginationAndListOfGenres()
    {
        var defaultPagingParams = new PagingParams();
        var response = await _client.GetAsync("api/genres");

        response.EnsureSuccessStatusCode();

        var genresPaginated = await response.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>();

        // NOTE: the default pageSize must be less than default number of entities to generate
        Assert.NotNull(genresPaginated);
        Assert.Equal(defaultPagingParams.PageSize, genresPaginated.Items.Count());
    }

    [Fact]
    public async Task Get_WithExplicitPagination_FirstPageNotEqualSecondPage()
    {
        var paginatedResponse1 = await _client.GetAsync("api/genres?pageSize=10&page=1");
        var paginatedResponse2 = await _client.GetAsync("api/genres?pageSize=10&page=2");

        paginatedResponse1.EnsureSuccessStatusCode();
        paginatedResponse2.EnsureSuccessStatusCode();

        var genrePaginated1 = paginatedResponse1.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>();
        var genrePaginated2 = paginatedResponse2.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>();

        Assert.NotNull(genrePaginated1);
        Assert.NotNull(genrePaginated2);
        Assert.NotEqual(genrePaginated1, genrePaginated2);
    }

    [Fact]
    public async Task Get_WithDifferentPageSize_NumberOfItemsInPageIsDifferent()
    {
        int pageSize1 = 10;
        int pageSize2 = 20;
        int page = 1;

        var response1 = await _client.GetAsync($"api/genres?pageSize={pageSize1}&page={page}");
        var response2 = await _client.GetAsync($"api/genres?pageSize={pageSize2}&page={page}");

        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();

        var paginatedResponse1 = await response1.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>();
        var paginatedResponse2 = await response2.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>();

        Assert.NotEqual(pageSize1, pageSize2);
        Assert.NotNull(paginatedResponse1);
        Assert.NotNull(paginatedResponse2);
        Assert.NotEqual(paginatedResponse1!.Items.Count(), paginatedResponse2!.Items.Count());
    }

    [Fact]
    public async Task Get_WithPageSizeNotSpecifiedAndPageSpecified_ResponseHasDefaultPageSize()
    {
        int page = 1;
        var defaultPagingParams = new PagingParams();

        var response1 = await _client.GetAsync($"api/genres?page={page}");
        var response2 = await _client.GetAsync($"api/genres?page={page}");

        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();

        var paginatedResponse1 = await response1.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>();
        var paginatedResponse2 = await response2.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>();

        Assert.NotNull(paginatedResponse1);
        Assert.NotNull(paginatedResponse2);
        Assert.Equal(defaultPagingParams.PageSize, paginatedResponse1.Items.Count());
        Assert.Equal(paginatedResponse1!.Items.Count(), paginatedResponse2!.Items.Count());
    }

    [Fact]
    public async Task Get_WithPageSizeSpecifiedAndPageNotSpecified_ResponseHasDefaultPage()
    {
        int pageSize1 = 10;
        int pageSize2 = 20;

        var defaultPagingParams = new PagingParams();

        var response1 = await _client.GetAsync($"api/genres?pageSize={pageSize1}");
        var response2 = await _client.GetAsync($"api/genres?pageSize={pageSize2}");

        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();

        var paginatedResponse1 = await response1.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>();
        var paginatedResponse2 = await response2.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>();

        Assert.NotNull(paginatedResponse1);
        Assert.NotNull(paginatedResponse2);
        Assert.NotEqual(paginatedResponse2.Items.Count(), paginatedResponse1.Items.Count());
        Assert.Equal(paginatedResponse1.Pagination.Page, paginatedResponse2.Pagination.Page);
    }

    [Fact]
    public async Task Get_GenreWithParentIdPresentQueried_ReturnsGenreWithSpecifiedParentId()
    {
        var parentGenre = GenGenreCreateDto();

        var parentResponse = await _client.PostAsJsonAsync("api/genres", parentGenre);

        parentResponse.EnsureSuccessStatusCode();

        var createdParent = await parentResponse.Content.ReadFromJsonAsync<GenreReadDto>();

        Assert.NotNull(createdParent);

        var childGenre = new GenreCreateDto
        {
            Name = "Child Genre",
            Description = "Child Description",
            ParentId = createdParent.Id
        };

        var childResponse = await _client.PostAsJsonAsync("api/genres", childGenre);

        childResponse.EnsureSuccessStatusCode();

        var queriedChildGenreResponse = await _client.GetAsync($"api/genres?parentId={createdParent.Id}");

        queriedChildGenreResponse.EnsureSuccessStatusCode();

        var queriedChildren =
            (await queriedChildGenreResponse.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>())!.Items;

        Assert.Single(queriedChildren);
    }

    [Fact]
    public async Task Get_GenreWithQPresentQueried_ReturnsGenreWithSpecifiedQ()
    {
        var searchedTerm = "searchedTerm";
        var firstGenre = GenGenreCreateDto();
        var secondGenre = GenGenreCreateDto();
        firstGenre.Name = $"Name contain {searchedTerm}";
        secondGenre.Description = $"Description contain {searchedTerm}";

        var firstResponse = await _client.PostAsJsonAsync("api/genres", firstGenre);
        var firstCreated = await firstResponse.Content.ReadFromJsonAsync<GenreReadDto>();

        firstResponse.EnsureSuccessStatusCode();

        var secondResponse = await _client.PostAsJsonAsync("api/genres", firstGenre);
        var secondCreated = await secondResponse.Content.ReadFromJsonAsync<GenreReadDto>();

        secondResponse.EnsureSuccessStatusCode();

        var queriedQGenreResponse = await _client.GetAsync($"api/genres?q={searchedTerm}");
        var queriedGetResponse =
            (await queriedQGenreResponse.Content.ReadFromJsonAsync<PaginatedResponse<GenreReadDto>>())!.Items.ToList();

        queriedQGenreResponse.EnsureSuccessStatusCode();

        Assert.Equal(2, queriedGetResponse.Count());
        Assert.Equal(queriedGetResponse[0].Id, firstCreated!.Id);
        Assert.Equal(queriedGetResponse[1].Id, secondCreated!.Id);
    }
}