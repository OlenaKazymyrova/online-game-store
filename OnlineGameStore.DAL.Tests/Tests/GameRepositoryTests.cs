using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Tests.Creators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class GameRepositoryTests
{
    private readonly UnitOfWorkCreator<Game> _creatorUnitOfWork = new UnitOfWorkCreator<Game>();
    private readonly IUnitOfWork _unitOfWork;

    public GameRepositoryTests()
    {
        _unitOfWork = _creatorUnitOfWork.CreateUnitOfWork();
    }

    [Fact]
    public async Task Add_SavesGameToDatabase()
    {

        var game = new Game
        {
            Name = "Test Game",
            Description = "Test Description",
            Publisher = Guid.NewGuid(),
            Genre = Guid.NewGuid()
        };


        await _unitOfWork.Games.Add(game);
        var changes = _unitOfWork.Save();


        Assert.Equal(1, changes);
        var result = await _unitOfWork.Games.GetById(game.Id);
        Assert.NotNull(result);
        Assert.Equal(game.Name, result.Name);
    }

    [Fact]
    public async Task GetById_ReturnsCorrectGame()
    {

        var game = new Game
        {
            Name = "Test Game",
            Description = "Test Description"
        };
        await _unitOfWork.Games.Add(game);
        _unitOfWork.Save();


        var result = await _unitOfWork.Games.GetById(game.Id);


        Assert.NotNull(result);
        Assert.Equal(game.Id, result.Id);
    }

    [Fact]
    public async Task Get_ReturnsAllGames()
    {

        var game1 = new Game
        {
            Name = "Test Game 1",
            Description = "Test Description 1",
            Publisher = Guid.NewGuid(),
            Genre = Guid.NewGuid()
        };
        var game2 = new Game
        {
            Name = "Test Game 2",
            Description = "Test Description 2",
            Publisher = Guid.NewGuid(),
            Genre = Guid.NewGuid()
        };
        await _unitOfWork.Games.Add(game1);
        await _unitOfWork.Games.Add(game2);
        _unitOfWork.Save();


        var result = await _unitOfWork.Games.Get();


        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Update_ModifiesExistingGame()
    {

        var game = new Game
        {
            Name = "Test Game",
            Description = "Test Description",
            Publisher = Guid.NewGuid(),
            Genre = Guid.NewGuid()
        };
        await _unitOfWork.Games.Add(game);
        _unitOfWork.Save();


        game.Name = "Updated Name";
        _unitOfWork.Games.Update(game);
        _unitOfWork.Save();
        var updatedGame = await _unitOfWork.Games.GetById(game.Id);


        Assert.Equal("Updated Name", updatedGame.Name);
    }

    [Fact]
    public async Task Delete_RemovesGameFromDatabase()
    {

        var game = new Game
        {
            Name = "Test Game",
            Description = "Test Description",
            Publisher = Guid.NewGuid(),
            Genre = Guid.NewGuid()
        };
        await _unitOfWork.Games.Add(game);
        _unitOfWork.Save();


        _unitOfWork.Games.DeleteById(game.Id);
        _unitOfWork.Save();
        var result = await _unitOfWork.Games.GetById(game.Id);


        Assert.Null(result);
    }
}
