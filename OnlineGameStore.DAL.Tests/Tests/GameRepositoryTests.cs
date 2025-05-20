using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Tests;

public class GameRepositoryTests
{
    private readonly GameRepositoryCreator _creator = new GameRepositoryCreator();

    [Fact]
    public async Task AddAsync_AddsGame()
    {
        var repository = _creator.Create();
        var game = new Game
        {
            Name = "Test Game",
            Description = "Test Description",
            Publisher = 1,
            Genre = 1,
            License = 1
        };
        var result = await repository.AddAsync(game);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(game.Name, result.Name);
        Assert.Equal(game.Description, result.Description);
        Assert.Equal(game.Publisher, result.Publisher);
        Assert.Equal(game.Genre, result.Genre);
        Assert.Equal(game.License, result.License);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGame()
    {
        var repository = _creator.Create();
        var game = new Game
        {
            Name = "Test Game",
            Description = "Test Description",
            Publisher = 1,
            Genre = 1,
            License = 1
        };
        var addedGame = await repository.AddAsync(game);
        var result = await repository.GetByIdAsync(addedGame!.Id);

        Assert.NotNull(result);
        Assert.Equal(addedGame.Id, result.Id);
        Assert.Equal(addedGame.Name, result.Name);
        Assert.Equal(addedGame.Description, result.Description);
        Assert.Equal(game.Publisher, result.Publisher);
        Assert.Equal(game.Genre, result.Genre);
        Assert.Equal(game.License, result.License);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllGames()
    {
        var repository = _creator.Create();
        var game1 = new Game
        {
            Name = "Test Game 1",
            Description = "Test Description 1",
            Publisher = 1,
            Genre = 1,
            License = 1
        };
        var game2 = new Game
        {
            Name = "Test Game 2",
            Description = "Test Description 2",
            Publisher = 2,
            Genre = 2,
            License = 2
        };
        await repository.AddAsync(game1);
        await repository.AddAsync(game2);

        var result = await repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesGame()
    {
        var repository = _creator.Create();
        var game = new Game
        {
            Name = "Test Game",
            Description = "Test Description",
            Publisher = 1,
            Genre = 1,
            License = 1
        };
        var addedGame = await repository.AddAsync(game);
        addedGame!.Name = "Updated Game";
        var result = await repository.UpdateAsync(addedGame);

        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteAsync_DeletesGame()
    {
        var repository = _creator.Create();
        var game = new Game
        {
            Name = "Test Game",
            Description = "Test Description",
            Publisher = 1,
            Genre = 1,
            License = 1
        };
        var addedGame = await repository.AddAsync(game);
        var result = await repository.DeleteAsync(addedGame!.Id);

        Assert.True(result);
    }
}