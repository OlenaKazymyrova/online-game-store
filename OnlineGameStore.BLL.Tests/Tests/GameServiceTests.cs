using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Mapping;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Tests.Tests;

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
    public async Task GetByIdAsync_GameExists_ReturnsGame()
    {
        var game = _data[0];

        var result = await _gameService.GetByIdAsync(game.Id);

        Assert.NotNull(result);
        Assert.Equal(game.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_GameDoesNotExist_ReturnsNull()
    {
        var result = await _gameService.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsGame()
    {
        var newGame = new GameDto
        {
            Id = Guid.NewGuid(),
            Name = "New Game",
            Description = "Action",
            PublisherId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid()
        };

        var result = await _gameService.AddAsync(newGame);

        Assert.NotNull(result);
        Assert.Equal(newGame.Id, result.Id);
    }
}