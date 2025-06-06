﻿using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Tests.RepositoryCreators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class GameRepositoryTests
{
    private readonly GameRepositoryCreator _creator = new GameRepositoryCreator();

    [Fact]
    public async Task AddAsync_AddsGame()
    {
        var repository = _creator.Create();
        var game = GetGame();
        var result = await repository.AddAsync(game);

        Assert.NotNull(result);
        Assert.True(result.Id != Guid.Empty);
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
        var game = GetGame();
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
        var game1 = GetGame();
        var game2 = GetGame();
        await repository.AddAsync(game1);
        await repository.AddAsync(game2);

        var result = await repository.GetAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesGame()
    {
        var repository = _creator.Create();
        var game = GetGame();
        var addedGame = await repository.AddAsync(game);
        addedGame!.Name = "Updated Game";
        var result = await repository.UpdateAsync(addedGame);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_DeletesGame()
    {
        var repository = _creator.Create();
        var game = GetGame();
        var addedGame = await repository.AddAsync(game);
        var result = await repository.DeleteAsync(addedGame!.Id);

        Assert.True(result);
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
            Publisher = Guid.NewGuid(),
            Genre = Guid.NewGuid(),
            License = Guid.NewGuid(),
            Price = price,
            ReleaseDate = releaseDate
        };
    }
}