using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Mapping;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.BLL.Tests.Tests;

public class GameServiceTests
{
    private const int EntityCount = 100;
    private readonly GameService _gameService;
    private readonly List<Game> _data;
    private readonly IMapper _mapper;

    public GameServiceTests()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<BllMappingProfile>(); });

        _mapper = config.CreateMapper();

        var gen = new GameEntityGenerator();

        _data = gen.Generate(EntityCount);
        var repMock = new GameRepositoryMockCreator(_data);

        var mockRepository = repMock.Create();

        _gameService = new GameService(mockRepository, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_GameExists_ReturnsGame()
    {
        var game = _data[0];

        var returnedGame = await _gameService.GetByIdAsync(game.Id);

        Assert.NotNull(returnedGame);
        Assert.Equal(game.Id, returnedGame.Id);
    }

    [Fact]
    public async Task GetByIdAsync_GameDoesNotExist_ReturnsNull()
    {
        var emptyGame = await _gameService.GetByIdAsync(Guid.NewGuid());

        Assert.Null(emptyGame);
    }

    [Fact]
    public async Task GetAsync_GamesExist_ReturnsDefaultPaginatedGames()
    {
        var pagingParams = new PagingParams();
        var gamesPaginated = await _gameService.GetAsync();

        int skip = (pagingParams.Page - 1) * pagingParams.PageSize;
        var dataPaginatedExpected = _data.Skip(skip).Take(pagingParams.PageSize);

        Assert.NotNull(gamesPaginated);
        Assert.Equal(dataPaginatedExpected.Count(), gamesPaginated.Items.Count());
    }

    [Fact]
    public async Task AddAsync_ReturnsGame()
    {
        var newGame = GetGameDto();

        var created = await _gameService.AddAsync(newGame);

        Assert.NotNull(created);
        Assert.Equal(newGame.Id, created.Id);
    }

    [Fact]
    public async Task DeleteAsync_GameExists_ReturnsTrue()
    {
        var game = _data[0];
        var result = await _gameService.DeleteAsync(game.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_GameDoesNotExist_ReturnsFalse()
    {
        var result = await _gameService.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_GameAlreadyDeleted_ReturnsFalse()
    {
        var game = _data[0];
        await _gameService.DeleteAsync(game.Id);

        var result = await _gameService.DeleteAsync(game.Id);

        Assert.False(result);
    }

    private GameDto GetGameDto(
        string name = "Test Game",
        string description = "Test Description",
        decimal price = 59.99m,
        DateTime releaseDate = default)
    {
        return new GameDto
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            PublisherId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid(),
            Price = price,
            ReleaseDate = releaseDate
        };
    }
}