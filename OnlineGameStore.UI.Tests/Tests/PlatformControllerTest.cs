using System.Net;
using System.Net.Http.Json;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.DTOs.Platform;
using OnlineGameStore.UI.Tests.DataGenerators;
using OnlineGameStore.UI.Tests.ServiceMockCreators;

namespace OnlineGameStore.UI.Tests.Tests;

public class PlatformControllerTests
{
    private readonly HttpClient _client;
    private const int DtoCount = 100;

    public PlatformControllerTests()
    {
        var data = new PlatformDtoDataGenerator().Generate(DtoCount);
        var mockCreator = new PlatformServiceMockCreator(data);

        var factory = new ControllerTestsHelper<IPlatformService>(mockCreator);
        _client = factory.CreateClient();
    }

    
    [Fact]
    public async Task GetByIdAsync_PlatformValid_ReturnsPlatform()
    {
        var newPlatform = new PlatformRequestDto
        {
            Name = "New Platform",
        };

        var postRequest = await _client.PostAsJsonAsync("/platforms", newPlatform);
        Assert.Equal(HttpStatusCode.Created, postRequest.StatusCode);

        var platform = await postRequest.Content.ReadFromJsonAsync<PlatformResponseDto>();
        var platformId = platform!.Id;
        
        var getRequest = await _client.GetAsync($"/platforms/{platformId}");
      
        Assert.Equal(HttpStatusCode.OK, getRequest.StatusCode);

        var fetchedPlatform = await getRequest.Content.ReadFromJsonAsync<PlatformResponseDto>();
        Assert.NotNull(fetchedPlatform);
        Assert.Equal(platformId, fetchedPlatform.Id);
        Assert.Equal(newPlatform.Name, fetchedPlatform.Name);
    }
    
    [Fact]
    public async Task GetByIdAsync_PlatformDoesNotExist_ReturnsNotFound()
    {
        
        var nonExistentId = Guid.NewGuid();
        
        var response = await _client.GetAsync($"/platforms/{nonExistentId}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllPlatforms()
    {
        var response = await _client.GetAsync("/platforms");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var platforms = await response.Content.ReadFromJsonAsync<IEnumerable<PlatformResponseDto>>();
        Assert.NotNull(platforms);
        Assert.NotEmpty(platforms);
        Assert.Equal(DtoCount, platforms.Count());
    }
    
    [Fact]
    public async Task CreateAsync_ValidPlatform_ReturnsCreatedPlatform()
    {
        var newPlatform = new PlatformRequestDto { Name = "New Platform" };
        
        var response = await _client.PostAsJsonAsync("/platforms", newPlatform);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var createdPlatform = await response.Content.ReadFromJsonAsync<PlatformResponseDto>();
        Assert.NotNull(createdPlatform);
        Assert.Equal(newPlatform.Name, createdPlatform.Name);
        Assert.NotEqual(Guid.Empty, createdPlatform.Id);
    }
    
    [Fact]
    public async Task CreateAsync_DuplicatePlatformName_ReturnsConflict()
    { 
        
        var platformName = "Duplicate Platform";
        var firstPlatform = new PlatformRequestDto { Name = platformName };
        await _client.PostAsJsonAsync("/platforms", firstPlatform);

        var duplicatePlatform = new PlatformRequestDto { Name = platformName };
        
        var response = await _client.PostAsJsonAsync("/platforms", duplicatePlatform);
        
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateAsync_InvalidPlatform_ReturnsBadRequest()
    {
        var invalidPlatform = new PlatformRequestDto { Name = "" }; 
        
        var response = await _client.PostAsJsonAsync("/platforms", invalidPlatform);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidPlatform_ReturnsUpdatedPlatform()
    {
        var originalPlatform = new PlatformRequestDto { Name = "Original Platform" };
        var postResponse = await _client.PostAsJsonAsync("/platforms", originalPlatform);
        var createdPlatform = await postResponse.Content.ReadFromJsonAsync<PlatformResponseDto>();

        var updatedPlatform = new PlatformRequestDto { Name = "Updated Platform" };
        
        var putResponse = await _client.PutAsJsonAsync($"/platforms/{createdPlatform!.Id}", updatedPlatform);
        
        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        
        var resultPlatform = await putResponse.Content.ReadFromJsonAsync<PlatformResponseDto>();
        Assert.NotNull(resultPlatform);
        Assert.Equal(updatedPlatform.Name, resultPlatform.Name);
        Assert.Equal(createdPlatform.Id, resultPlatform.Id);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentPlatform_ReturnsNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var platformUpdate = new PlatformRequestDto { Name = "Non-existent Platform" };

        var response = await _client.PutAsJsonAsync($"/platforms/{nonExistentId}", platformUpdate);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateAsync_DuplicatePlatformName_ReturnsConflict()
    {
        
        var firstPlatform = new PlatformRequestDto { Name = "First Platform" };
        await _client.PostAsJsonAsync("/platforms", firstPlatform);
        
        var secondPlatform = new PlatformRequestDto { Name = "Second Platform" };
        var secondPostResponse = await _client.PostAsJsonAsync("/platforms", secondPlatform);
        var secondCreatedPlatform = await secondPostResponse.Content.ReadFromJsonAsync<PlatformResponseDto>();

        var updateToDuplicateName = new PlatformRequestDto { Name = "First Platform" };
        
        var response = await _client.PutAsJsonAsync($"/platforms/{secondCreatedPlatform!.Id}", updateToDuplicateName);
        
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteAsync_ExistingPlatform_ReturnsNoContent()
    {
        var newPlatform = new PlatformRequestDto { Name = "Platform to Delete" };
        var postResponse = await _client.PostAsJsonAsync("/platforms", newPlatform);
        var createdPlatform = await postResponse.Content.ReadFromJsonAsync<PlatformResponseDto>();
        
        var deleteResponse = await _client.DeleteAsync($"/platforms/{createdPlatform!.Id}");
        
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        
        var getResponse = await _client.GetAsync($"/platforms/{createdPlatform.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteAsync_NonExistentPlatform_ReturnsNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        
        var response = await _client.DeleteAsync($"/platforms/{nonExistentId}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}