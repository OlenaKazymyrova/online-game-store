using OnlineGameStore.DAL.Tests.RepositoryCreators;

namespace OnlineGameStore.DAL.Tests.Tests;

public class PlatformRepositoryTests
{
    private readonly PlatformRepositoryCreator _creator = new ();
    
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
        
        var result = await repository.GetAsync();
        
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAsync_WithFilter_ReturnsFilteredPlatforms()
    {
        var repository = _creator.Create();
        var platform1 = GetPlatform("Xbox");
        var platform2 = GetPlatform("PlayStation");
        await repository.AddAsync(platform1);
        await repository.AddAsync(platform2);
        
        var result = await repository.GetAsync(p => p.Name.Contains("box"));
        
        Assert.Single(result);
        Assert.Equal("Xbox", result.First().Name);
    }

    [Fact]
    public async Task GetAsync_WithOrderBy_ReturnsOrderedPlatforms()
    {
        var repository = _creator.Create();
        var platform1 = GetPlatform("B Platform");
        var platform2 = GetPlatform("A Platform");
        await repository.AddAsync(platform1);
        await repository.AddAsync(platform2);
        
        var result = await repository.GetAsync(orderBy: q => q.OrderBy(p => p.Name));
        
        Assert.Equal(2, result.Count());
        Assert.Equal("A Platform", result.First().Name);
        Assert.Equal("B Platform", result.Last().Name);
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
    public async Task DeleteAsync_ReturnsFalseForNonExistentId()
    {
        var repository = _creator.Create();
        var nonExistentId = Guid.NewGuid();
       
        var result = await repository.DeleteAsync(nonExistentId);
        
        Assert.False(result);
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