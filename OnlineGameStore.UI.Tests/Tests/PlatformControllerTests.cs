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
    public async Task PostPlatform_ValidData_ReturnsCreated()
    {
        var newPlatformDto = GenPlatformCreateDto();

        var response = await _client.PostAsJsonAsync("api/Platforms", newPlatformDto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdPlatform = await response.Content.ReadFromJsonAsync<PlatformDto>();
        var location = response.Headers.Location;

        Assert.NotNull(createdPlatform);
        Assert.NotNull(location);
        Assert.EndsWith($"api/Platforms/{createdPlatform.Id}", location.ToString());
        Assert.Equal(newPlatformDto.Name, createdPlatform!.Name);
        Assert.NotEqual(Guid.Empty, createdPlatform.Id);
    }

    [Fact]
    public async Task PostPlatform_MissingName_ReturnsBadRequest()
    {
        var invalidDto = new PlatformCreateDto { Name = "" };

        var response = await _client.PostAsJsonAsync("api/Platforms", invalidDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostPlatform_MissingGameIds_ReturnsBadRequest()
    {
        var invalidDto = new PlatformCreateDto { Name = "Valid name", GamesIds = null };

        var response = await _client.PostAsJsonAsync("api/Platforms", invalidDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


    [Fact]
    public async Task PostPlatform_DuplicateName_ReturnsConflict()
    {
        var dto = GenPlatformCreateDto();

        var firstResponse = await _client.PostAsJsonAsync("api/Platforms", dto);
        firstResponse.EnsureSuccessStatusCode();

        var secondResponse = await _client.PostAsJsonAsync("api/Platforms", dto);

        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task UpdatePlatform_ValidData_ReturnsUpdatedPlatform()
    {
        var platformCreateDto = GenPlatformCreateDto();

        var postResponse = await _client.PostAsJsonAsync("api/Platforms", platformCreateDto);
        
        postResponse.EnsureSuccessStatusCode();

        var createdPlatform = await postResponse.Content.ReadFromJsonAsync<PlatformDto>();

        platformCreateDto.Name = "Updated name";

        var putResponse = await _client.PutAsJsonAsync($"api/Platforms/{createdPlatform!.Id}", platformCreateDto);

        putResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"api/Platforms/{createdPlatform.Id}");

        getResponse.EnsureSuccessStatusCode();

        var updatedPlatform = await getResponse.Content.ReadFromJsonAsync<PlatformDto>();

        Assert.NotNull(updatedPlatform);
        Assert.Equal(updatedPlatform.Id, createdPlatform.Id);
        Assert.NotEqual(updatedPlatform.Name, createdPlatform.Name);
    }
}