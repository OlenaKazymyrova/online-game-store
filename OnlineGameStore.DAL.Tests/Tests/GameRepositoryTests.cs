using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Migrations;
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
        Assert.Equivalent(game, result, strict: true);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGame()
    {
        var repository = _gameRepoCreator.Create();
        var game = GetGame();
        var addedGame = await repository.AddAsync(game);
        var result = await repository.GetByIdAsync(addedGame!.Id);

        Assert.NotNull(result);
        Assert.Equivalent(addedGame, result, strict: true);
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

    [Fact]
    public async Task UpdatePlatformRefsAsync_GameIsPresentPlatformPresent_UpdatesGame()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new OnlineGameStoreDbContext(options);
        var gameRepo = (GameRepository)Activator.CreateInstance(typeof(GameRepository), context)!;
        var platformRepo = (PlatformRepository)Activator.CreateInstance(typeof(PlatformRepository), context)!;

        var game = GetGame();

        Assert.NotNull(await gameRepo.AddAsync(game));

        var platform = new Platform
        {
            Id = Guid.NewGuid(),
            Name = "name"
        };

        Assert.NotNull(await platformRepo.AddAsync(platform));

        var ex = await Record.ExceptionAsync(() =>
            gameRepo.UpdatePlatformRefsAsync(game.Id, new List<Platform> { platform }));

        Assert.Null(ex);

        var updated = await gameRepo.GetByIdAsync(game.Id);

        Assert.Single(updated!.Platforms);
        Assert.Equal(platform.Id, updated.Platforms.First().Id);
    }

    [Fact]
    public async Task UpdatePlatformRefsAsync_GameNotFound_ThrowsKeyNotFoundException()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var ctx = new OnlineGameStoreDbContext(options);
        var repo = new GameRepository(ctx);

        var missingGameId = Guid.NewGuid();
        var dummyPlatform = new Platform { Id = Guid.NewGuid(), Name = "X" };

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            repo.UpdatePlatformRefsAsync(missingGameId, new List<Platform> { dummyPlatform }));

        Assert.Contains($"Could not find the Game with ID {missingGameId}", ex.Message);
    }

    [Fact]
    public async Task UpdatePlatformRefsAsync_PlatformNotFound_ThrowsDbUpdateConcurrencyException()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var ctx = new OnlineGameStoreDbContext(options);
        var gameRepo = new GameRepository(ctx);

        var game = GetGame();
        await gameRepo.AddAsync(game);

        var missingPlatform = new Platform
        {
            Id = Guid.NewGuid(),
            Name = "Nonexistent"
        };

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
            gameRepo.UpdatePlatformRefsAsync(game.Id, new List<Platform> { missingPlatform }));
    }

    [Fact]
    public async Task UpdateGenreRefsAsync_GameNotFound_ThrowsKeyNotFoundException()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var ctx = new OnlineGameStoreDbContext(options);
        var gameRepo = new GameRepository(ctx);

        var missingGameId = Guid.NewGuid();
        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "abac",
            Description = "abac",
            ParentId = null
        };

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            gameRepo.UpdateGenreRefsAsync(missingGameId, new List<Genre> { genre }));

        Assert.Contains($"Could not find the Game with ID {missingGameId}", ex.Message);
    }

    [Fact]
    public async Task UpdateGenreRefsAsync_GenreNotFound_ThrowsDbUpdateConcurrencyException()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var ctx = new OnlineGameStoreDbContext(options);
        var gameRepo = new GameRepository(ctx);

        var game = GetGame();
        await gameRepo.AddAsync(game);

        var missingGenre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "abac",
            Description = "abac",
        };

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
        gameRepo.UpdateGenreRefsAsync(game.Id, new List<Genre> { missingGenre }));
    }

    [Fact]
    public async Task UpdateGenreRefsAsync_GameFoundGenreFound_UpdatesGame()
    {

        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new OnlineGameStoreDbContext(options);
        var gameRepo = (GameRepository)Activator.CreateInstance(typeof(GameRepository), context)!;
        var genreRepo = (GenreRepository)Activator.CreateInstance(typeof(GenreRepository), context)!;

        var game = GetGame();
        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "abac",
            Description = "abac",
            ParentId = null
        };

        Assert.NotNull(await gameRepo.AddAsync(game));
        Assert.NotNull(await genreRepo.AddAsync(genre));

        var ex = await Record.ExceptionAsync(() =>
            gameRepo.UpdateGenreRefsAsync(game.Id, new List<Genre> { genre }));

        Assert.Null(ex);

        var updated = await gameRepo.GetByIdAsync(game.Id);

        Assert.NotNull(updated);
        Assert.Single(updated.Genres);
        Assert.Equal(genre.Id, updated.Genres.First().Id);
    }

    private Game GetGame(
        Guid? id = null,
        string name = "Test Game",
        string description = "Test Description",
        decimal price = 59.99m,
        DateTime releaseDate = default)
    {
        return new Game
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            Description = description,
            PublisherId = Guid.NewGuid(),
            LicenseId = Guid.NewGuid(),
            Price = price,
            ReleaseDate = releaseDate
        };
    }
}