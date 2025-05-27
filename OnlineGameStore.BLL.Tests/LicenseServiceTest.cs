using AutoMapper;
using Moq;
using Microsoft.AspNetCore.JsonPatch;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.Tests.Services
{
    public class LicenseServiceTests
    {
        private readonly Mock<ILicenseRepository> _licenseRepositoryMock;
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly IMapper? _mapper;
        private readonly LicenseService _licenseService;

        public LicenseServiceTests()
        {
            _licenseRepositoryMock = new Mock<ILicenseRepository>();
            _gameRepositoryMock = new Mock<IGameRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<License, LicenseResponseDto>();
                cfg.CreateMap<LicenseDto, License>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.GameId, opt => opt.Ignore())
                    .ForMember(dest => dest.Game, opt => opt.Ignore());
                cfg.CreateMap<License, LicenseDto>();
            });
            _mapper = config.CreateMapper();

            _licenseService = new LicenseService(
                _licenseRepositoryMock.Object,
                _gameRepositoryMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedLicenses()
        {
            var licenses = new List<License> { new License { Id = Guid.NewGuid(), Cost = 10, GameId = Guid.NewGuid() } };
            _licenseRepositoryMock.Setup(r => r.GetAsync(null, null, null)).ReturnsAsync(licenses);

            var result = await _licenseService.GetAllAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedLicense()
        {
            var id = Guid.NewGuid();
            var license = new License { Id = id, Cost = 20, GameId = Guid.NewGuid() };
            _licenseRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(license);

            var result = await _licenseService.GetByIdAsync(id);

            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsIfNotFound()
        {
            var id = Guid.NewGuid();
            _licenseRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((License)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _licenseService.GetByIdAsync(id));
        }

        [Fact]
        public async Task CreateAsync_ThrowsIfDtoIsNull()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _licenseService.CreateAsync(null));
        }

        [Fact]
        public async Task CreateAsync_ThrowsIfGameNotFound()
        {
            var dto = new LicenseDto { GameId = Guid.NewGuid(), Cost = 15 };
            _gameRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Game, bool>>>(), null, null))
                .ReturnsAsync((IEnumerable<Game>)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _licenseService.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ThrowsIfGameAlreadyHasLicense()
        {
            var dto = new LicenseDto { GameId = Guid.NewGuid(), Cost = 15 };

            _gameRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Game, bool>>>(), null, null))
                .ReturnsAsync(new List<Game> { new Game{
                    Name = "Test Game",
                    Description = "Test Description"
                } });

            _licenseRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<License, bool>>>(), null, null))
                .ReturnsAsync(new List<License> { new License() });

            await Assert.ThrowsAsync<ArgumentException>(() => _licenseService.CreateAsync(dto));
        }


        [Fact]
        public async Task CreateAsync_CreatesLicense()
        {
            var dto = new LicenseDto { GameId = Guid.NewGuid(), Cost = 15, Description = "Test" };

            _gameRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Game, bool>>>(), null, null))
                .ReturnsAsync(new List<Game> {
                    new Game { Name = "Test Game", Description = "Test Description" }
                });

            _licenseRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<License, bool>>>(), null, null))
                .ReturnsAsync(new List<License>());

            _licenseRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<License>()))
                .ReturnsAsync((License license) => license);

            var result = await _licenseService.CreateAsync(dto);

            Assert.Equal(dto.Cost, result.Cost);
            Assert.Equal(dto.Description, result.Description);
            Assert.Equal(dto.GameId, result.GameId);
        }


        [Fact]
        public async Task UpdateAsync_ThrowsIfNotFound()
        {
            var id = Guid.NewGuid();
            _licenseRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((License)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _licenseService.UpdateAsync(id, new LicenseDto()));
        }

        [Fact]
        public async Task UpdateAsync_CallsUpdate()
        {
            var id = Guid.NewGuid();
            var license = new License { Id = id, GameId = Guid.NewGuid(), Cost = 25 };
            _licenseRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(license);

            await _licenseService.UpdateAsync(id, new LicenseDto { Cost = 30, Description = "Updated" });

            _licenseRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<License>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsIfNotFound()
        {
            var id = Guid.NewGuid();
            _licenseRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((License)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _licenseService.DeleteAsync(id));
        }

        [Fact]
        public async Task DeleteAsync_CallsDelete()
        {
            var id = Guid.NewGuid();
            _licenseRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new License { Id = id });

            await _licenseService.DeleteAsync(id);

            _licenseRepositoryMock.Verify(r => r.DeleteByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task PatchAsync_ThrowsIfLicenseNotFound()
        {
            var id = Guid.NewGuid();
            _licenseRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((License)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _licenseService.PatchAsync(id, new JsonPatchDocument<LicenseDto>()));
        }

        [Fact]
        public async Task PatchAsync_UpdatesAndReturnsPatchedLicense()
        {
            var id = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var license = new License { Id = id, GameId = gameId, Cost = 50 };
            _licenseRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(license);
            _licenseRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<License>())).Returns(Task.CompletedTask);

            var patchDoc = new JsonPatchDocument<LicenseDto>();
            patchDoc.Replace(x => x.Description, "New Desc");

            var result = await _licenseService.PatchAsync(id, patchDoc);

            Assert.Equal("New Desc", result.Description);
        }
    }
}
