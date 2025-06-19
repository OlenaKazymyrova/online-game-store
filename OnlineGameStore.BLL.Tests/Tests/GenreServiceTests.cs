using AutoMapper;
using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.BLL.Mapping.Profiles;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.BLL.Tests.Tests;

public class GenreServiceTests
{
    private const int EntityCount = 100;
    private readonly GenreService _genreService;
    private readonly List<Genre> _data;
    readonly IMapper _mapper;

    public GenreServiceTests()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<BllGenreMappingProfile>(); });

        _mapper = config.CreateMapper();

        var gen = new GenreEntityGenerator();

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
        Assert.Equal(genre.Id, result.Id);
        Assert.Equal(genre.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_GenreDoesNotExist_ReturnsNull()
    {
        var result = await _genreService.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddValidGenreCreateDto_ReturnsValidGenreReadDto()
    {
        var newGenreDto = new GenreCreateDto
        {
            Name = "New name",
            Description = "Description",
            ParentId = null
        };

        var result = await _genreService.AddAsync(newGenreDto);

        Assert.NotNull(result);
        Assert.Equal(newGenreDto.Name, result.Name);
        Assert.Equal(newGenreDto.Description, result.Description);
    }

    [Fact]
    public async Task UpdateAsync_GenreExists_Updates()
    {
        var genre = _data[0];

        var genreCreateDto = new GenreCreateDto
        {
            Name = genre.Name,
            Description = "Updated description",
            ParentId = genre.ParentId
        };

        var result = await _genreService.UpdateAsync(genre.Id, genreCreateDto);

        var updatedGenre = await _genreService.GetByIdAsync(genre.Id);

        Assert.True(result);
        Assert.NotNull(updatedGenre);
        Assert.Equal(updatedGenre.Description, genreCreateDto.Description);
        Assert.Equal(genre.Name, updatedGenre.Name);
    }

    [Fact]
    public async Task GetAsync_WithoutExplicitPagination_ReturnsDefaultPaginatedResponse()
    {
        var pagingParams = new PagingParams();
        var result = await _genreService.GetAsync();

        int skip = (pagingParams.Page - 1) * pagingParams.PageSize;
        var dataPaginatedExpected = _data.Skip(skip).Take(pagingParams.PageSize);

        Assert.NotNull(result);
        Assert.Equal(dataPaginatedExpected.Count(), result.Items.Count());

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