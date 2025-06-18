using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.DataGenerators;
using OnlineGameStore.UI.Tests.ServiceMockCreators;
using System.Net;
using System.Net.Http.Json;
using OnlineGameStore.SharedLogic.Pagination;

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
    public async Task PostPlatform_MissingGameIds_ReturnsBadRequest()
    {
        var invalidDto = new PlatformCreateDto { Name = "Valid name", GamesIds = null! };

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
}
