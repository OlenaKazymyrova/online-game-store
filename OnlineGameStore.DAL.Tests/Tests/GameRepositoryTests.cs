using System.Xml;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Repositories;
using OnlineGameStore.DAL.Tests.RepositoryCreators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class GameRepositoryTests
{
    private readonly GameRepositoryCreator _gameRepoCreator = new GameRepositoryCreator();

    [Fact]
    public async Task AddAsync_AddsGame()
    {
        var repository = _gameRepoCreator.Create();
        var game = GetGame();
        var result = await repository.AddAsync(game);

        Assert.NotNull(result);
        Assert.True(result.Id != Guid.Empty);
        Assert.Equal(game.Name, result.Name);
        Assert.Equal(game.Description, result.Description);
        Assert.Equal(game.PublisherId, result.PublisherId);
        Assert.Equal(game.Platforms, result.Platforms);
        Assert.Equal(game.LicenseId, result.LicenseId);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGame()
    {
        var repository = _gameRepoCreator.Create();
        var game = GetGame();
        var addedGame = await repository.AddAsync(game);
        var result = await repository.GetByIdAsync(addedGame!.Id);

        Assert.NotNull(result);
        Assert.Equal(addedGame.Id, result.Id);
        Assert.Equal(addedGame.Name, result.Name);
        Assert.Equal(addedGame.Description, result.Description);
        Assert.Equal(game.PublisherId, result.PublisherId);
        Assert.Equal(game.Platforms, result.Platforms);
        Assert.Equal(game.Genres, result.Genres);
        Assert.Equal(game.LicenseId, result.LicenseId);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllGames()
    {
        var repository = _gameRepoCreator.Create();
        var game1 = GetGame();
        var game2 = GetGame();
        await repository.AddAsync(game1);
        await repository.AddAsync(game2);

        var paginatedResult = await repository.GetAsync();

        Assert.NotNull(paginatedResult);
        Assert.Equal(2, paginatedResult.Items.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesGame()
    {
        var repository = _gameRepoCreator.Create();
        var game = GetGame();
        var addedGame = await repository.AddAsync(game);
        addedGame!.Name = "Updated Game";
        var result = await repository.UpdateAsync(addedGame);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_DeletesGame()
    {
        var repository = _gameRepoCreator.Create();
        var game = GetGame();
        var addedGame = await repository.AddAsync(game);
        var result = await repository.DeleteAsync(addedGame!.Id);

        Assert.True(result);
    }

    // #####################
    // # Tests for relations are conducted on the same DbContext
    // #####################
    [Fact]
    public async Task AddAsync_GameWithNewGenreReference_CreatesNewGenre()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new OnlineGameStoreDbContext(options);
        var genreRepository = (GenreRepository)Activator.CreateInstance(typeof(GenreRepository), context)!;
        var gameRepository = (GameRepository)Activator.CreateInstance(typeof(GameRepository), context)!;

        var game = GetGame();
        game.Platforms.Add(new Platform
        {
            Id = Guid.NewGuid(),
            Name = "NonExistingPlatform"
        });
        game.Genres.Add(new Genre
        {
            Id = Guid.NewGuid(),
            Name = "NonExistingGenre",
            ParentId = null
        });

        var result = await gameRepository.AddAsync(game);
        var addedGenre = await genreRepository.GetByIdAsync(game.Genres.First().Id);

        Assert.NotNull(addedGenre);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddAsync_GameWithValidGenre_Success()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new OnlineGameStoreDbContext(options);
        var genreRepo = (GenreRepository)Activator.CreateInstance(typeof(GenreRepository), context)!;
        var gameRepo = (GameRepository)Activator.CreateInstance(typeof(GameRepository), context)!;

        var game = GetGame();
        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Test Genre",
            ParentId = null
        };

        var addedGenre = await genreRepo.AddAsync(genre);

        Assert.NotNull(addedGenre);

        game.Genres.Add(addedGenre);

        var addedGame = await gameRepo.AddAsync(game);

        Assert.NotNull(addedGame);
    }

    [Fact]
    public async Task AddAsync_GameWithNewPlatformReference_CreatesNewPlatform()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new OnlineGameStoreDbContext(options);
        var gameRepo = (GameRepository)Activator.CreateInstance(typeof(GameRepository), context)!;
        var platformRepo = (PlatformRepository)Activator.CreateInstance(typeof(PlatformRepository), context)!;

        var game = GetGame();
        game.Platforms.Add(new Platform
        {
            Id = Guid.NewGuid(),
            Name = "Test Platform"
        });

        var addedGame = await gameRepo.AddAsync(game);

        Assert.NotNull(addedGame);

        var addedPlatform = await platformRepo.GetByIdAsync(game.Platforms.First().Id);

        Assert.NotNull(addedPlatform);
    }

    private Game GetGame(
        string name = "Test Game",
        string description = "Test Description",
        decimal price = 59.99m,
        DateTime releaseDate = default)
    {
        return new Game
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            PublisherId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid(),
            Price = price,
            ReleaseDate = releaseDate
        };
    }
}