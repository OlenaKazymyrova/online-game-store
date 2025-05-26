using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Repositories;
using Xunit;

namespace OnlineGameStore.Tests.Repositories
{
    public class PlatformRepositoryTests
    {
        private readonly DbContextOptions<OnlineGameStoreDbContext> _dbContextOptions;

        public PlatformRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private (PlatformRepository repository, OnlineGameStoreDbContext context) CreateRepository()
        {
            var context = new OnlineGameStoreDbContext(_dbContextOptions);

            var mockFactory = new Mock<IOnlineGameStoreDbContextFactory>();
            mockFactory.Setup(f => f.CreateDbContext(It.IsAny<string[]>())).Returns(context);

            var repository = new PlatformRepository(mockFactory.Object);
            return (repository, context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddPlatform()
        {
            var (repo, context) = CreateRepository();
            var platform = new Platform { Id = Guid.NewGuid(), Name = "PC" };

            var result = await repo.AddAsync(platform);

            Assert.NotNull(result);
            Assert.Equal("PC", result.Name);
            Assert.Single(context.Platforms);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPlatform()
        {
            var (repo, context) = CreateRepository();
            var platform = new Platform { Id = Guid.NewGuid(), Name = "Xbox" };

            await context.Platforms.AddAsync(platform);
            await context.SaveChangesAsync();

            var result = await repo.GetByIdAsync(platform.Id);

            Assert.NotNull(result);
            Assert.Equal("Xbox", result.Name);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnFilteredAndOrdered()
        {
            var (repo, context) = CreateRepository();

            var platforms = new[]
            {
                new Platform { Id = Guid.NewGuid(), Name = "Xbox" },
                new Platform { Id = Guid.NewGuid(), Name = "PC" },
                new Platform { Id = Guid.NewGuid(), Name = "PlayStation" }
            };

            await context.Platforms.AddRangeAsync(platforms);
            await context.SaveChangesAsync();

            Expression<Func<Platform, bool>> filter = p => p.Name.Contains("P");
            Func<IQueryable<Platform>, IOrderedQueryable<Platform>> orderBy = q => q.OrderBy(p => p.Name);

            var result = await repo.GetAsync(filter, orderBy, null);

            Assert.Equal(2, result.Count());
            Assert.Collection(result,
                p => Assert.Equal("PC", p.Name),
                p => Assert.Equal("PlayStation", p.Name));
        }

        [Fact]
        public async Task GetAsync_ShouldIncludeGamePlatformsAndGames()
        {
            var (repo, context) = CreateRepository();

            var game = new Game
            {
                Id = Guid.NewGuid(),
                Name = "Game 1",
                Description = "A test game"
            };

            var platform = new Platform
            {
                Id = Guid.NewGuid(),
                Name = "Steam",
                GamePlatforms = new List<GamePlatform>()
            };

            var gamePlatform = new GamePlatform
            {
                GameId = game.Id,
                PlatformId = platform.Id,
                Game = game,
                Platform = platform
            };

            platform.GamePlatforms.Add(gamePlatform);
            game.GamePlatforms = new List<GamePlatform> { gamePlatform };

            await context.Games.AddAsync(game);
            await context.Platforms.AddAsync(platform);
            await context.SaveChangesAsync();

            Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>> include = q =>
                q.Include(p => p.GamePlatforms).ThenInclude(gp => gp.Game);

            var result = await repo.GetAsync(include: include);
            var loadedPlatform = result.First();


            Assert.Equal("Game 1", loadedPlatform.GamePlatforms.First().Game.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePlatform()
        {
            var (repo, context) = CreateRepository();
            var platform = new Platform { Id = Guid.NewGuid(), Name = "OldName" };

            await context.Platforms.AddAsync(platform);
            await context.SaveChangesAsync();

            platform.Name = "NewName";
            await repo.UpdateAsync(platform);

            var updated = await context.Platforms.FindAsync(platform.Id);
            Assert.Equal("NewName", updated.Name);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldDeletePlatform()
        {
            var (repo, context) = CreateRepository();
            var platform = new Platform { Id = Guid.NewGuid(), Name = "DeleteMe" };

            await context.Platforms.AddAsync(platform);
            await context.SaveChangesAsync();

            await repo.DeleteByIdAsync(platform.Id);
            var deleted = await context.Platforms.FindAsync(platform.Id);

            Assert.Null(deleted);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeletePlatform()
        {
            var (repo, context) = CreateRepository();
            var platform = new Platform { Id = Guid.NewGuid(), Name = "DeleteDirectly" };

            await context.Platforms.AddAsync(platform);
            await context.SaveChangesAsync();

            await repo.DeleteAsync(platform);
            var deleted = await context.Platforms.FindAsync(platform.Id);

            Assert.Null(deleted);
        }
    }
}
