using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.DataGenerators;
using OnlineGameStore.UI.Tests.ServiceMockCreators;
using OnlineGameStore.SharedLogic.Pagination;
using OnlineGameStore.BLL.DTOs.Platforms;
using System.Net;
using System.Net.Http.Json;

namespace OnlineGameStore.UI.Tests.Tests;

public class PlatformsControllerTests
{
    private const int _dtoAmountToGenerate = 100;
    private readonly HttpClient _client;

    public PlatformsControllerTests()
    {
        var data = new PlatformEntityGenerator().Generate(_dtoAmountToGenerate);
        var mockCreator = new PlatformServiceMockCreator(data);
        var factory = new ControllerTestsHelper<IPlatformService>(mockCreator);
        _client = factory.CreateClient();
    }

    private static PlatformCreateDto GenPlatformCreateDto()
    {
        var platformGen = new PlatformCreateDtoGenerator();
        return platformGen.Generate(1).First();
    }

    private static (PlatformCreateDto first, PlatformCreateDto second) GenPlatformCreateDtos()
    {
        var platformGen = new PlatformCreateDtoGenerator();
        var list = platformGen.Generate(2);
        return (list[0], list[1]);
    }

    [Fact]
    public async Task PostPlatform_ValidData_ReturnsCreated()
    {
        var newPlatformDto = GenPlatformCreateDto();

        var response = await _client.PostAsJsonAsync("api/platforms", newPlatformDto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdPlatform = await response.Content.ReadFromJsonAsync<PlatformDto>();
        var location = response.Headers.Location;

        Assert.NotNull(createdPlatform);
        Assert.NotNull(location);
        Assert.EndsWith($"api/platforms/{createdPlatform.Id}", location.ToString());
        Assert.Equal(newPlatformDto.Name, createdPlatform!.Name);
        Assert.NotEqual(Guid.Empty, createdPlatform.Id);
    }

    [Fact]
    public async Task PostPlatform_MissingName_ReturnsBadRequest()
    {
        var invalidDto = new PlatformCreateDto { Name = "" };

        var response = await _client.PostAsJsonAsync("api/platforms", invalidDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostPlatform_DuplicateName_ReturnsConflict()
    {
        var dto = GenPlatformCreateDto();

        var firstResponse = await _client.PostAsJsonAsync("api/platforms", dto);
        firstResponse.EnsureSuccessStatusCode();

        var secondResponse = await _client.PostAsJsonAsync("api/Platforms", dto);

        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task GetByIdPlatform_ValidData_ReturnsPlatform()
    {
        var newPlatformDto = GenPlatformCreateDto();

        var postResponse = await _client.PostAsJsonAsync("api/platforms", newPlatformDto);

        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var platform = await postResponse.Content.ReadFromJsonAsync<PlatformDto>();
        var platformId = platform!.Id;

        var getResponse = await _client.GetAsync($"api/platforms/{platformId}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetchedPlatform = await getResponse.Content.ReadFromJsonAsync<PlatformDto>();

        Assert.NotNull(fetchedPlatform);
        Assert.Equal(platformId, fetchedPlatform.Id);
    }

    [Fact]
    public async Task GetByIdPlatform_DoesNotExist_ReturnNotFound()
    {
        var newId = Guid.NewGuid().ToString();
        var getResponse = await _client.GetAsync($"api/platforms/{newId}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Get_WithoutExplicitPagination_GetsJsonDefaultPaginationAndListOfPlatforms()
    {
        var defaultPaginationParams = new PagingParams();
        var getResponse = await _client.GetAsync("api/platforms");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var platformsPaginatedGetResponse =
            await getResponse.Content.ReadFromJsonAsync<PaginatedResponse<PlatformDto>>();

        // NOTE: the default pageSize must be less than default number of entities to generate
        Assert.NotNull(platformsPaginatedGetResponse);
        Assert.Equal(defaultPaginationParams.PageSize, platformsPaginatedGetResponse.Items.Count());
    }

    [Fact]
    public async Task Get_WithInvalidPagination_ReturnsBadRequest()
    {
        var invalidPage = 0;
        var invalidPageSize = 200;

        var getResponse = await _client.GetAsync($"api/platforms" + $"?page={invalidPage}&pageSize={invalidPageSize}");

        Assert.Equal(HttpStatusCode.BadRequest, getResponse.StatusCode);

        var content = await getResponse.Content.ReadAsStringAsync();

        Assert.Contains("Page must be greater than 0", content);
    }

    [Fact]
    public async Task DeletePlatform_PlatformExists_ReturnsNoContent()
    {
        var newPlatform = GenPlatformCreateDto();

        var postRequest = await _client.PostAsJsonAsync("api/platforms", newPlatform);

        postRequest.EnsureSuccessStatusCode();

        var createdPlatform = await postRequest.Content.ReadFromJsonAsync<PlatformDto>();

        var deleteResponse = await _client.DeleteAsync($"api/platforms/{createdPlatform!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task DeletePlatform_PlatformDoesNotExist_ReturnsNotFound()
    {
        var deleteRequest = await _client.DeleteAsync($"api/platforms/{Guid.NewGuid().ToString()}");

        Assert.Equal(HttpStatusCode.NotFound, deleteRequest.StatusCode);
    }

    [Fact]
    public async Task UpdatePutPlatform_ValidData_ReturnsUpdatedPlatform()
    {
        var platformCreateDto = GenPlatformCreateDto();

        var postResponse = await _client.PostAsJsonAsync("api/platforms", platformCreateDto);

        postResponse.EnsureSuccessStatusCode();

        var createdPlatform = await postResponse.Content.ReadFromJsonAsync<PlatformDto>();

        platformCreateDto.Name = "Updated name";

        var putResponse = await _client.PutAsJsonAsync($"api/platforms/{createdPlatform!.Id}", platformCreateDto);

        putResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"api/platforms/{createdPlatform!.Id}");

        getResponse.EnsureSuccessStatusCode();

        var updatedPlatform = await getResponse.Content.ReadFromJsonAsync<PlatformDto>();

        Assert.NotNull(updatedPlatform);
        Assert.Equal(updatedPlatform.Id, createdPlatform.Id);
        Assert.NotEqual(updatedPlatform.Name, createdPlatform.Name);
    }

    [Fact]
    public async Task UpdatePutPlatform_PlatformDoesNotExist_ReturnsNotFound()
    {
        var platformCreateDto = GenPlatformCreateDto();

        var putResponse = await _client.PutAsJsonAsync($"api/platforms/{Guid.NewGuid()}", platformCreateDto);

        Assert.Equal(HttpStatusCode.NotFound, putResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateAsync_DuplicatePlatformName_ReturnsConflict()
    {
        var (firstPlatform, secondPlatform) = GenPlatformCreateDtos();

        var firstPostResponse = await _client.PostAsJsonAsync("api/platforms", firstPlatform);
        var secondPostResponse = await _client.PostAsJsonAsync("api/platforms", secondPlatform);

        firstPostResponse.EnsureSuccessStatusCode();
        secondPostResponse.EnsureSuccessStatusCode();

        var firstCreatedPlatform = await firstPostResponse.Content.ReadFromJsonAsync<PlatformDto>();
        var secondCreatedPlatform = await secondPostResponse.Content.ReadFromJsonAsync<PlatformDto>();
        secondCreatedPlatform!.Name = firstCreatedPlatform!.Name;

        var response =
            await _client.PutAsJsonAsync($"api/platforms/{secondCreatedPlatform!.Id}", secondCreatedPlatform);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
