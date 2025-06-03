using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Mapping;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Tests.Tests;

public class GenreServiceTests
{
    private const int EntityCount = 100;
    private readonly GenreService _genreService;
    private readonly List<Genre> _data;
    readonly IMapper _mapper;

    public GenreServiceTests()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<BllMappingProfile>(); });

        _mapper = config.CreateMapper();

        var gen = new GenreDataGenerator();

        _data = gen.Generate(EntityCount);
        var repMock = new GenreRepositoryMockCreator(_data);

        var mockRepository = repMock.Create();

        _genreService = new GenreService(mockRepository, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_GameExists_ReturnsGame()
    {
        var genre = _data[0];

        var result = await _genreService.GetByIdAsync(genre.Id);

        Assert.NotNull(result);
        Assert.NotNull(genre);
        Assert.Equal(genre, _mapper.Map<Genre>(result));
    }

    [Fact]
    public async Task GetByIdAsync_GenreDoesNotExist_ReturnsNull()
    {
        var result = await _genreService.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsGame()
    {
        var newGenre = new GenreDto
        {
            Id = Guid.NewGuid(),
            Name = "New name",
            Description = "Description",
            ParentId = null
        };

        var result = await _genreService.AddAsync(newGenre);

        Assert.NotNull(result);
        Assert.Equal(newGenre, result);
    }

    [Fact]
    public async Task UpdateAsync_GenreExists_Updates()
    {
        var genre = _data[0];

        genre.Description = "Updated description";
        genre.Name = "Updated name";

        var result = await _genreService.UpdateAsync(_mapper.Map<GenreDto>(genre));

        Assert.True(result);
    }

    [Fact]
    public async Task GetAll_ReturnsAll()
    {
        var result = await _genreService.GetAllAsync();

        Assert.Equal(_data, _mapper.Map<List<Genre>>(result));
    }

    [Fact]
    public async Task DeleteAsync_GenreExists_Deletes()
    {
        var genre = _data[0];

        var resultBeforeDeletion = await _genreService.DeleteAsync(genre.Id);

        var resultAfterDeletion = await _genreService.DeleteAsync(genre.Id);


        Assert.True(resultBeforeDeletion);
        Assert.False(resultAfterDeletion);
    }
}

