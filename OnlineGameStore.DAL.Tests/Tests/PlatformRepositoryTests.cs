using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Repositories;
using OnlineGameStore.DAL.Tests.RepositoryCreators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class PlatformRepositoryTests
{
    private readonly PlatformRepositoryCreator _creator = new();

    [Fact]
    public async Task AddAsync_AddsPlatform()
    {
        var repository = _creator.Create();
        var platform = GetPlatform();

        var result = await repository.AddAsync(platform);

        Assert.NotNull(result);
        Assert.True(result.Id != Guid.Empty);
        Assert.Equal(platform.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPlatform()
    {
        var repository = _creator.Create();
        var platform = GetPlatform();
        var addedPlatform = await repository.AddAsync(platform);

        var result = await repository.GetByIdAsync(addedPlatform!.Id);

        Assert.NotNull(result);
        Assert.Equal(addedPlatform.Id, result.Id);
        Assert.Equal(addedPlatform.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullForNonExistentId()
    {
        var repository = _creator.Create();
        var nonExistentId = Guid.NewGuid();

        var result = await repository.GetByIdAsync(nonExistentId);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ReturnsAllPlatforms()
    {
        var repository = _creator.Create();
        var platform1 = GetPlatform("Platform 1");
        var platform2 = GetPlatform("Platform 2");
        await repository.AddAsync(platform1);
        await repository.AddAsync(platform2);

        var paginatedResult = await repository.GetAsync();

        Assert.NotNull(paginatedResult);
        Assert.Equal(2, paginatedResult.Items.Count());
    }

    [Fact]
    public async Task GetAsync_WithFilter_ReturnsFilteredPlatforms()
    {
        var repository = _creator.Create();
        var platform1 = GetPlatform("Xbox");
        var platform2 = GetPlatform("PlayStation");
        await repository.AddAsync(platform1);
        await repository.AddAsync(platform2);

        var paginatedResult = await repository.GetAsync(p => p.Name.Contains("box"));

        Assert.Single(paginatedResult.Items);
        Assert.Equal("Xbox", paginatedResult.Items.First().Name);
    }

    [Fact]
    public async Task GetAsync_WithOrderBy_ReturnsOrderedPlatforms()
    {
        var repository = _creator.Create();
        var platform1 = GetPlatform("B Platform");
        var platform2 = GetPlatform("A Platform");
        await repository.AddAsync(platform1);
        await repository.AddAsync(platform2);

        var paginatedResult = await repository.GetAsync(orderBy: q => q.OrderBy(p => p.Name));

        Assert.Equal(2, paginatedResult.Items.Count());
        Assert.Equal("A Platform", paginatedResult.Items.First().Name);
        Assert.Equal("B Platform", paginatedResult.Items.Last().Name);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesPlatform()
    {
        var repository = _creator.Create();
        var platform = GetPlatform();
        var addedPlatform = await repository.AddAsync(platform);
        addedPlatform!.Name = "Updated Platform Name";

        var result = await repository.UpdateAsync(addedPlatform);
        var updatedPlatform = await repository.GetByIdAsync(addedPlatform.Id);

        Assert.True(result);
        Assert.Equal("Updated Platform Name", updatedPlatform!.Name);
    }

    [Fact]
    public async Task DeleteAsync_DeletesPlatform()
    {
        var repository = _creator.Create();
        var platform = GetPlatform();
        var addedPlatform = await repository.AddAsync(platform);

        var result = await repository.DeleteAsync(addedPlatform!.Id);
        var deletedPlatform = await repository.GetByIdAsync(addedPlatform.Id);

        Assert.True(result);
        Assert.Null(deletedPlatform);
    }

    [Fact]
    public async Task DeleteAsync_NonExistentId_ThrowsArgumentNullException()
    {
        var repository = _creator.Create();
        var nonExistentId = Guid.NewGuid();

        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await repository.DeleteAsync(nonExistentId));
    }

    [Fact]
    public async Task GetByIdAsync_PlatformWithGamesReferencePresent_ReturnsPlatfromWithReference()
    {
        var repository = _creator.Create();
        var platform = GetPlatform();
        platform.Games.Add(new Game
        {
            Id = Guid.NewGuid(),
            Name = "cool game",
            Description = "cool desc",
            Price = 0,
            ReleaseDate = DateTime.Now
        });

        var addedPlatform = await repository.AddAsync(platform);

        var retrievedPlatform = await repository.GetByIdAsync(addedPlatform!.Id);

        Assert.NotEmpty(retrievedPlatform!.Games);
    }

    private Platform GetPlatform(string name = "Test Platform")
    {
        return new Platform
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
}