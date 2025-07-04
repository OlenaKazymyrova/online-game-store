using AutoMapper;
using OnlineGameStore.BLL.DTOs.Platforms;
using OnlineGameStore.BLL.Exceptions;
using OnlineGameStore.BLL.Mapping.Profiles;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Tests.Tests;

public class PlatformServiceTests
{
    private readonly PlatformService _platformService;
    private readonly List<Platform> _data;
    private readonly IMapper _mapper;

    public PlatformServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BllPlatformMappingProfile>());
        _mapper = config.CreateMapper();

        var gen = new PlatformEntityGenerator();
        _data = gen.Generate(10);

        var repoMock = new PlatformRepositoryMockCreator(_data);
        var mockRepository = repoMock.Create();

        _platformService = new PlatformService(mockRepository, _mapper);
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
    public async Task GetByIdAsync_PlatformDoesNotExist_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(async () => await _platformService.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task AddAsync_NewPlatform_ReturnsPlatformDto()
    {
        var newPlatform = new PlatformCreateDto { Name = "New Platform" };
        var result = await _platformService.AddAsync(newPlatform);

        Assert.NotNull(result);
        Assert.Equal(newPlatform.Name, result!.Name);
    }

    [Fact]
    public async Task AddAsync_ExistingPlatformName_ThrowsValidationException()
    {
        var existing = _data[0];
        var duplicateDto = new PlatformCreateDto { Name = existing.Name };

        var exception =
            await Assert.ThrowsAsync<Exceptions.ValidationException>(() => _platformService.AddAsync(duplicateDto));

        Assert.Equal("Platform name already exists.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_UniqueName_ReturnsTrue()
    {
        var platform = _data[0];
        var updateDto = new PlatformCreateDto { Name = "Updated Unique Name" };

        var result = await _platformService.UpdateAsync(platform.Id, updateDto);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_DuplicateName_ThrowsConflictException()
    {
        var first = _data[0];
        var second = _data[1];

        var updateDto = new PlatformCreateDto { Name = first.Name };
        var exception =
            await Assert.ThrowsAsync<ConflictException>(() => _platformService.UpdateAsync(second.Id, updateDto));

        Assert.Equal("Platform name already exists.", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_PlatformExists_ReturnsTrue()
    {
        var platform = _data[0];
        var result = await _platformService.DeleteAsync(platform.Id);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_PlatformDoesNotExist_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(async () => await _platformService.DeleteAsync(Guid.NewGuid()));
    }
}