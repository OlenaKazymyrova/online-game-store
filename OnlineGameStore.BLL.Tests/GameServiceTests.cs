using AutoMapper;
using OnlineGameStore.BLL.Mapping;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Tests;

public class GameServiceTests
{
    private const int EntityCount = 100;
    private readonly GameService _gameService;
    private readonly List<Game> _data;

    public GameServiceTests()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        var mapper = config.CreateMapper();

        var gen = new GameDataGenerator();

        _data = gen.Generate(EntityCount);
        var repMock = new GameRepositoryMockCreator(_data);

        var mockRepository = repMock.Create();

        _gameService = new GameService(mockRepository, mapper);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnGame_WhenGameExists()
    {
        var game = _data[0];
        
        var result = await _gameService.GetByIdAsync(game.Id);
        
        Assert.NotNull(result);
        Assert.Equal(game.Id, result.Id);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenGameDoesNotExist()
    {
        var result = await _gameService.GetByIdAsync(Guid.NewGuid());
        
        Assert.Null(result);
    }
}