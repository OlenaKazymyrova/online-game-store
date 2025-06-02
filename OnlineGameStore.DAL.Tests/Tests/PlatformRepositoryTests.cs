using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Repositories;
using OnlineGameStore.DAL.Tests.RepositoryCreators;
using OnlineGameStore.SharedLogic.DataGenerators.DataEntityGenerators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class PlatformRepositoryTests
{
    private readonly PlatformRepositoryCreator _creator = new PlatformRepositoryCreator();
    private readonly PlatformGenerator _generator = new PlatformGenerator();

    [Fact]
    public async Task AddAsync_ShouldAddPlatform()
    {
        var repo = _creator.Create();
        var platform = _generator.Generate(1)[0];

        var result = await repo.AddAsync(platform);

        Assert.NotNull(result);
        Assert.True(result.Id != Guid.Empty);
        Assert.Equal(platform.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPlatform()
    {
        var repo = _creator.Create();
        var platform = _generator.Generate(1)[0];

        var addedPlatform = await repo.AddAsync(platform);

        var result = await repo.GetByIdAsync(addedPlatform!.Id);

        Assert.NotNull(result);
        Assert.Equal(addedPlatform.Name, result.Name);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnFilteredAndOrdered()
    {
        var repo = _creator.Create();

        var platforms = _generator.Generate(3);

        foreach (var platform in platforms)
        {
            await repo.AddAsync(platform);
        }

        Expression<Func<Platform, bool>> filter = p => !p.Name.Contains("2");
        Func<IQueryable<Platform>, IOrderedQueryable<Platform>> orderBy = q => q.OrderBy(p => p.Name);

        var result = await repo.GetAllAsync(filter, orderBy, null);

        Assert.Equal(2, result.Count());
        Assert.Collection(result,
            p => Assert.Equal("Platform 0", p.Name),
            p => Assert.Equal("Platform 1", p.Name));
    }

    [Fact]
    public async Task GetAsync_ShouldIncludeGamePlatformsAndGames()
    {
        var platformRepo = _creator.Create();
        var gameRepo = new GameRepositoryCreator().Create();

        var game = new GameGenerator().Generate(1)[0];

        var platform = _generator.Generate(1)[0];

        var gamePlatform = new GamePlatform
        {
            GameId = game.Id,
            PlatformId = platform.Id,
            Game = game,
            Platform = platform
        };

        platform.GamePlatforms.Add(gamePlatform);
        game.GamePlatforms.Add(gamePlatform);

        await gameRepo.AddAsync(game);
        await platformRepo.AddAsync(platform);

        Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>> include = q =>
            q.Include(p => p.GamePlatforms).ThenInclude(gp => gp.Game);

        var result = await platformRepo.GetAllAsync(include: include);
        var loadedPlatform = result.First();


        Assert.Equal("Game 0", loadedPlatform.GamePlatforms.First().Game.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePlatform()
    {
        var repo = _creator.Create();
        var platform = _generator.Generate(1)[0];

        await repo.AddAsync(platform);

        platform.Name = "NewName";

        var result = await repo.UpdateAsync(platform);
        var updatedPlatform = await repo.GetByIdAsync(platform.Id);

        Assert.True(result);
        Assert.Equal("NewName", updatedPlatform.Name);
    }


    [Fact]
    public async Task DeleteAsync_ShouldDeletePlatform()
    {
        var repo = _creator.Create();
        var platform = _generator.Generate(1)[0];

        var addedPlatform = await repo.AddAsync(platform);

        var result = await repo.DeleteAsync(addedPlatform!.Id);
        var deletedPlatform = await repo.GetByIdAsync(platform.Id);

        Assert.True(result);
        Assert.Null(deletedPlatform);
    }

}