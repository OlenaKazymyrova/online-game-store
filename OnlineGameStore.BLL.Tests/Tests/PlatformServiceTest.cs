using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Mapping;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.SharedLogic.DataGenerators.DataEntityGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Tests.Tests;

public class PlatformServiceTests
{ 
    private const int EntityCount = 100;
    private readonly PlatformService _platformService;
    private readonly List<Platform> _data;

    public PlatformServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BllMappingProfile>());
        var mapper = config.CreateMapper();

        var gen = new PlatformGenerator();
        
        _data = gen.Generate(EntityCount);
        var repMock = new PlatformRepositoryMockCreator(_data);
            
        var mockRepository = repMock.Create();

        _platformService = new PlatformService(mockRepository, mapper);
    }

    [Fact]
    public async Task GetByIdAsync_PlatformExists_ReturnsPlatform()
    { 
        var platform = _data[0];
        
        var result = await _platformService.GetByIdAsync(platform.Id);

        Assert.NotNull(result);
        Assert.Equal(platform.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_PlatformDoesNotExist_ReturnsNull()
    { 
        var result = await _platformService.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPlatforms()
    {
        var result = await _platformService.GetAllAsync();
        
        Assert.NotNull(result); 
        Assert.Equal(EntityCount, result.Count());
    }

    [Fact] 
    public async Task AddAsync_ValidPlatform_ReturnsCreatedPlatform() 
    { 
        var newPlatform = new PlatformDto
        {
            Name = "New Platform"
        };

        var result = await _platformService.AddAsync(newPlatform);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id); 
        Assert.Equal(101,_data.Count);
    }

    [Fact]
    public async Task UpdateAsync_PlatformExists_ReturnsUpdatedPlatform()
    {
       
        var existingPlatform = _data[0];
        var updateDto = new PlatformDto()
        {
            Id = existingPlatform.Id,
            Name = "Updated Platform"
        };
        
        var result = await _platformService.UpdateAsync(updateDto);
        
        var updatedPlatform = await _platformService.GetByIdAsync(existingPlatform.Id);
        
        Assert.True(result);
        Assert.NotNull(updatedPlatform);
        Assert.Equal("Updated Platform", updatedPlatform.Name);
    }

    [Fact]
    public async Task UpdateAsync_PlatformDoesNotExist_ReturnsNull()
    {
        var updateDto = new PlatformDto()
        {
            Id = Guid.NewGuid(),
            Name = "Updated Platform "
        };

        var result = await _platformService.UpdateAsync(updateDto);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_PlatformExists_ReturnsTrue()
    {
        var platformToDelete = _data[0];

        var result = await _platformService.DeleteAsync(platformToDelete.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_PlatformDoesNotExist_ReturnsFalse()
    {
        var result = await _platformService.DeleteAsync(Guid.NewGuid());
        
        Assert.False(result);
    }
}
