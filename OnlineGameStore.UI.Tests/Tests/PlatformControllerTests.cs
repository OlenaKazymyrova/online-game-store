using OnlineGameStore.DAL.Entities;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.DataGenerators;
using OnlineGameStore.UI.Tests.ServiceMockCreators;
using System.Net;
using System.Net.Http.Json;
using OnlineGameStore.SharedLogic.Pagination;
using Microsoft.AspNetCore.Mvc;
using Castle.Components.DictionaryAdapter.Xml;

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
    public async Task Getplatform_GenreExists_ReturnsGenre()
    {
        var newPlatformDto = GenPlatformCreateDto();
        var postResponse = await _client.PostAsJsonAsync("api/platforms", newPlatformDto);

        postResponse.EnsureSuccessStatusCode();

        var createdPlatform = await postResponse.Content.ReadFromJsonAsync<PlatformDto>();

        Assert.NotNull(createdPlatform);

        var getResponse = await _client.GetAsync($"api/platforms/{createdPlatform!.Id}");

        getResponse.EnsureSuccessStatusCode();

        var fetchedPlatform = await getResponse.Content.ReadFromJsonAsync<PlatformDto>();

        Assert.NotNull(fetchedPlatform);
        Assert.Equal(createdPlatform.Id, fetchedPlatform.Id);
        Assert.Equal(createdPlatform.Name, fetchedPlatform.Name);
    }

    [Fact]
    public async Task GetPlatform_PlatformNotExists_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var getResponse = await _client.GetAsync($"api/platforms/{id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task PostPlatform_ValidData_ReturnsCreated()
    {
        var newPlatformDto = GenPlatformCreateDto();

        var response = await _client.PostAsJsonAsync("api/platforms", newPlatformDto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdPlatform = await response.Content.ReadFromJsonAsync<PlatformDto>();

        Assert.NotNull(createdPlatform);
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

        var secondResponse = await _client.PostAsJsonAsync("api/platforms", dto);

        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }
}