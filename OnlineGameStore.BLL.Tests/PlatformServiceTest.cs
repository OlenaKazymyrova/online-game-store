using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Mappings;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using Xunit;

namespace OnlineGameStore.BLL.Tests
{
    public class PlatformServiceTests
    {
        private readonly Mock<IPlatformRepository> _platformRepoMock = new();
        private readonly Mock<IGameRepository> _gameRepoMock = new();
        private readonly IMapper _mapper;
        private readonly PlatformService _service;

        public PlatformServiceTests()
        {

            var config = new MapperConfiguration(cfg => cfg.AddProfile<PlatformProfile>());
            _mapper = config.CreateMapper();

            _service = new PlatformService(_platformRepoMock.Object, _gameRepoMock.Object, _mapper);
        }

        private static Platform CreatePlatform(Guid? id = null, string name = "Test Platform", List<Guid>? gameIds = null)
        {
            var platform = new Platform
            {
                Id = id ?? Guid.NewGuid(),
                Name = name,
                GamePlatforms = gameIds?.Select(gid => new GamePlatform { GameId = gid }).ToList() ?? new List<GamePlatform>()
            };
            return platform;
        }

        private static List<Guid> CreateGameIds(int count = 3)
        {
            return Enumerable.Range(1, count).Select(_ => Guid.NewGuid()).ToList();
        }

        private void SetupGameRepositoryForExistingGames(List<Guid> gameIds)
        {
            _gameRepoMock.Setup(gr => gr.GetAsync(
                    It.IsAny<Expression<Func<Game, bool>>>(),
                    null,
                    null))
                .ReturnsAsync(gameIds.Select(id => new Game
                {
                    Id = id,
                    Name = "Test Game",
                    Description = "Test Description"
                }));
        }






        [Fact]
        public async Task CreateAsync_Should_Create_And_Return_PlatformResponseDto()
        {

            var gameIds = CreateGameIds();
            var dto = new PlatformDto { Name = "New Platform", GameIds = gameIds };
            SetupGameRepositoryForExistingGames(gameIds);

            _platformRepoMock.Setup(pr => pr.AddAsync(It.IsAny<Platform>()))
                .ReturnsAsync((Platform p) => p);

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.GameIds.Count, result.GameIds.Count);
            _platformRepoMock.Verify(pr => pr.AddAsync(It.IsAny<Platform>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Platforms()
        {

            var platforms = new List<Platform>
            {
                CreatePlatform(gameIds: CreateGameIds()),
                CreatePlatform(gameIds: CreateGameIds())
            };

            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(platforms);

            var results = await _service.GetAllAsync();

            Assert.NotNull(results);
            Assert.Equal(platforms.Count, results.Count());

        }

        [Fact]
        public async Task GetById_Should_Return_PlatformResponseDto_When_Found()
        {

            var id = Guid.NewGuid();
            var platform = CreatePlatform(id, "Platform 1", CreateGameIds());
            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(new[] { platform });


            var result = await _service.GetById(id);


            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(platform.Name, result.Name);
        }

        [Fact]
        public async Task GetById_Should_Throw_KeyNotFoundException_When_Not_Found()
        {

            var id = Guid.NewGuid();

            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(Array.Empty<Platform>());


            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetById(id));
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Platform_When_Found()
        {

            var id = Guid.NewGuid();
            var existingGameIds = CreateGameIds();
            var platform = CreatePlatform(id, "Old Name", existingGameIds);

            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(new[] { platform });

            var updatedGameIds = CreateGameIds();
            var dto = new PlatformDto { Name = "Updated Name", GameIds = updatedGameIds };
            SetupGameRepositoryForExistingGames(updatedGameIds);

            _platformRepoMock.Setup(pr => pr.UpdateAsync(platform)).Returns(Task.CompletedTask);


            await _service.UpdateAsync(id, dto);


            Assert.Equal(dto.Name, platform.Name);
            Assert.Equal(updatedGameIds.Count, platform.GamePlatforms.Count);

        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_KeyNotFoundException_When_Platform_Not_Found()
        {

            var id = Guid.NewGuid();

            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(Array.Empty<Platform>());


            var dto = new PlatformDto { Name = "Name" };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(id, dto));
        }

        [Fact]
        public async Task PatchAsync_Should_Apply_Patch_And_Update()
        {

            var id = Guid.NewGuid();
            var gameIds = CreateGameIds();
            var platform = CreatePlatform(id, "Name", gameIds);

            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(new[] { platform });


            _platformRepoMock.Setup(pr => pr.UpdateAsync(platform)).Returns(Task.CompletedTask);
            SetupGameRepositoryForExistingGames(gameIds);

            var patchDoc = new JsonPatchDocument<PlatformDto>();
            patchDoc.Replace(p => p.Name, "Patched Name");

            await _service.PatchAsync(id, patchDoc);

            Assert.Equal("Patched Name", platform.Name);

        }

        [Fact]
        public async Task PatchAsync_Should_Throw_KeyNotFoundException_When_Platform_Not_Found()
        {

            var id = Guid.NewGuid();

            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(Array.Empty<Platform>());


            var patchDoc = new JsonPatchDocument<PlatformDto>();

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.PatchAsync(id, patchDoc));
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_When_Platform_Found()
        {

            var id = Guid.NewGuid();
            var platform = CreatePlatform(id);
            _platformRepoMock.Setup(pr => pr.GetByIdAsync(id)).ReturnsAsync(platform);
            _platformRepoMock.Setup(pr => pr.DeleteAsync(platform)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(id);


            _platformRepoMock.Verify(pr => pr.DeleteAsync(platform), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Not_Throw_When_Platform_Not_Found()
        {

            var id = Guid.NewGuid();
            _platformRepoMock.Setup(pr => pr.GetByIdAsync(id)).ReturnsAsync((Platform?)null);

            await _service.DeleteAsync(id);

            _platformRepoMock.Verify(pr => pr.DeleteAsync(It.IsAny<Platform>()), Times.Never);
        }

        [Fact]
        public async Task AddGamesToPlatform_Should_Add_New_Games_And_Update()
        {

            var id = Guid.NewGuid();
            var existingGameIds = CreateGameIds();
            var platform = CreatePlatform(id, "Platform", existingGameIds);

            var gamesToAdd = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            SetupGameRepositoryForExistingGames(gamesToAdd);

            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(new[] { platform });


            _platformRepoMock.Setup(pr => pr.UpdateAsync(platform)).Returns(Task.CompletedTask);

            await _service.AddGamesToPlatform(id, gamesToAdd);

            foreach (var gameId in gamesToAdd)
            {
                Assert.Contains(platform.GamePlatforms, gp => gp.GameId == gameId);
            }

        }

        [Fact]
        public async Task AddGamesToPlatform_Should_Do_Nothing_When_Null_Or_Empty_GameIds()
        {
            var id = Guid.NewGuid();

            await _service.AddGamesToPlatform(id, null);
            await _service.AddGamesToPlatform(id, new List<Guid>());

            _platformRepoMock.Verify(pr => pr.GetAsync(
                It.IsAny<Expression<Func<Platform, bool>>>(),
                null,
                It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
            ), Times.Never);
            _platformRepoMock.Verify(pr => pr.UpdateAsync(It.IsAny<Platform>()), Times.Never);
        }

        [Fact]
        public async Task AddGamesToPlatform_Should_Throw_When_Platform_Not_Found()
        {
            var id = Guid.NewGuid();
            var gamesToAdd = CreateGameIds();

            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(Array.Empty<Platform>());


            SetupGameRepositoryForExistingGames(gamesToAdd);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AddGamesToPlatform(id, gamesToAdd));
        }

        [Fact]
        public async Task RemoveGamesFromPlatform_Should_Remove_Games_And_Update()
        {
            var id = Guid.NewGuid();
            var gameIds = CreateGameIds();
            var platform = CreatePlatform(id, "Platform", gameIds);

            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(new[] { platform });

            _platformRepoMock.Setup(pr => pr.UpdateAsync(platform)).Returns(Task.CompletedTask);

            var gamesToRemove = gameIds.Take(2).ToList();

            await _service.RemoveGamesFromPlatform(id, gamesToRemove);

            foreach (var gameId in gamesToRemove)
            {
                Assert.DoesNotContain(platform.GamePlatforms, gp => gp.GameId == gameId);
            }

        }

        [Fact]
        public async Task RemoveGamesFromPlatform_Should_Do_Nothing_When_Null_Or_Empty_GameIds()
        {
            var id = Guid.NewGuid();

            await _service.RemoveGamesFromPlatform(id, null);
            await _service.RemoveGamesFromPlatform(id, new List<Guid>());

            _platformRepoMock.Verify(pr => pr.GetAsync(
                It.IsAny<Expression<Func<Platform, bool>>>(),
                null,
                It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
            ), Times.Never);
            _platformRepoMock.Verify(pr => pr.UpdateAsync(It.IsAny<Platform>()), Times.Never);
        }

        [Fact]
        public async Task RemoveGamesFromPlatform_Should_Throw_When_Platform_Not_Found()
        {
            var id = Guid.NewGuid();
            var gamesToRemove = CreateGameIds();

            _platformRepoMock.Setup(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    null,
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
                ))
                .ReturnsAsync(Array.Empty<Platform>());


            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.RemoveGamesFromPlatform(id, gamesToRemove));
        }

        [Fact]
        public async Task ReplaceGamesInPlatform_Should_Clear_And_Add_New_Games_And_Update()
        {

            var platformId = Guid.NewGuid();
            var oldGameIds = CreateGameIds();
            var platform = CreatePlatform(platformId, "Test Platform", oldGameIds);

            var newGameIds = CreateGameIds(2);

            _platformRepoMock.Setup(pr => pr.GetAsync(
                It.IsAny<Expression<Func<Platform, bool>>>(),
                null,
                It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()
            )).ReturnsAsync(new[] { platform });


            _platformRepoMock.Setup(pr => pr.UpdateAsync(platform)).Returns(Task.CompletedTask);


            await _service.ReplaceGamesInPlatform(platformId, newGameIds);


            Assert.Equal(newGameIds.Count, platform.GamePlatforms.Count);
            foreach (var newGameId in newGameIds)
            {
                Assert.Contains(platform.GamePlatforms, gp => gp.GameId == newGameId);
            }


        }


        [Fact]
        public async Task ReplaceGamesInPlatform_Should_Throw_KeyNotFoundException_If_Platform_Not_Found()
        {

            var platformId = Guid.NewGuid();
            var newGameIds = CreateGameIds();

            _platformRepoMock.Setup(pr => pr.GetAsync(
                It.IsAny<Expression<Func<Platform, bool>>>(),
                null,
                It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()  // include param
            )).ReturnsAsync(new List<Platform>());


            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.ReplaceGamesInPlatform(platformId, newGameIds));
        }

        [Fact]
        public async Task ReplaceGamesInPlatform_Should_Return_If_GameIds_Is_Null_Or_Empty()
        {

            var platformId = Guid.NewGuid();


            await _service.ReplaceGamesInPlatform(platformId, null);

            await _service.ReplaceGamesInPlatform(platformId, new List<Guid>());


            _platformRepoMock.Verify(pr => pr.GetAsync(
                    It.IsAny<Expression<Func<Platform, bool>>>(),
                    It.IsAny<Func<IQueryable<Platform>, IOrderedQueryable<Platform>>>(),
                    It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>()),
                Times.Never);
            _platformRepoMock.Verify(pr => pr.UpdateAsync(It.IsAny<Platform>()), Times.Never);
        }
    }

}
