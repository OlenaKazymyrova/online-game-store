using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Repositories;
using OnlineGameStore.DAL.Interfaces;  // Make sure this namespace contains IOnlineGameStoreDbContextFactory
using Xunit;

namespace OnlineGameStore.Tests.Repositories
{
    public class LicenseRepositoryTests
    {
        private readonly DbContextOptions<OnlineGameStoreDbContext> _dbContextOptions;

        public LicenseRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private (LicenseRepository repository, OnlineGameStoreDbContext context) CreateRepository()
        {
            var context = new OnlineGameStoreDbContext(_dbContextOptions);

            var mockFactory = new Mock<IOnlineGameStoreDbContextFactory>();
            mockFactory.Setup(f => f.CreateDbContext(It.IsAny<string[]>())).Returns(context);

            var repository = new LicenseRepository(mockFactory.Object);
            return (repository, context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            var (repo, context) = CreateRepository();

            var license = new License { Id = Guid.NewGuid(), Description = "License 1" };
            var result = await repo.AddAsync(license);

            Assert.NotNull(result);
            Assert.Equal(license.Description, result.Description);
            Assert.Equal(1, await context.Licenses.CountAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            var (repo, context) = CreateRepository();

            var license = new License { Id = Guid.NewGuid(), Description = "License 2" };
            await context.Licenses.AddAsync(license);
            await context.SaveChangesAsync();

            var result = await repo.GetByIdAsync(license.Id);

            Assert.NotNull(result);
            Assert.Equal(license.Description, result.Description);
        }

        [Fact]
        public async Task GetAsync_ShouldFilterOrderInclude()
        {
            var (repo, context) = CreateRepository();

            var licenses = new[]
            {
                new License { Id = Guid.NewGuid(), Description = "B License" },
                new License { Id = Guid.NewGuid(), Description = "A License" },
                new License { Id = Guid.NewGuid(), Description = "M License" },
            };

            await context.Licenses.AddRangeAsync(licenses);
            await context.SaveChangesAsync();

            Expression<Func<License, bool>> filter = l => l.Description.StartsWith("A") || l.Description.StartsWith("M");

            Func<IQueryable<License>, IOrderedQueryable<License>> orderBy = q => q.OrderBy(l => l.Description);

            Func<IQueryable<License>, IIncludableQueryable<License, object>>? include = null;

            var result = await repo.GetAsync(filter, orderBy, include);

            Assert.Equal(2, result.Count());
            Assert.Collection(result,
                item => Assert.StartsWith("A", item.Description),
                item => Assert.StartsWith("M", item.Description));
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyEntity()
        {
            var (repo, context) = CreateRepository();

            var license = new License { Id = Guid.NewGuid(), Description = "Old License" };
            await context.Licenses.AddAsync(license);
            await context.SaveChangesAsync();

            license.Description = "Updated License";
            await repo.UpdateAsync(license);

            var updated = await context.Licenses.FindAsync(license.Id);
            Assert.Equal("Updated License", updated.Description);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldRemoveEntity()
        {
            var (repo, context) = CreateRepository();

            var license = new License { Id = Guid.NewGuid(), Description = "To Delete By Id" };
            await context.Licenses.AddAsync(license);
            await context.SaveChangesAsync();

            await repo.DeleteByIdAsync(license.Id);

            var deleted = await context.Licenses.FindAsync(license.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            var (repo, context) = CreateRepository();

            var license = new License { Id = Guid.NewGuid(), Description = "To Delete" };
            await context.Licenses.AddAsync(license);
            await context.SaveChangesAsync();

            await repo.DeleteAsync(license);

            var deleted = await context.Licenses.FindAsync(license.Id);
            Assert.Null(deleted);
        }
    }
}
