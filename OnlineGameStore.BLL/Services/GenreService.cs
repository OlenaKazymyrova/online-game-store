using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.BLL.Services;

public class GenreService : Service<Genre, GenreCreateDto, GenreReadDto, GenreDto, GenreReadDto>, IGenreService
{
    public GenreService(IGenreRepository repository, IMapper mapper)
        : base(repository, mapper) { }

    public override async Task<PaginatedResponse<GenreReadDto>> GetAsync(
        Expression<Func<Genre, bool>>? filter = null,
        Func<IQueryable<Genre>, IOrderedQueryable<Genre>>? orderBy = null,
        Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>? include = null,
        PagingParams? pagingParams = null,
        HashSet<string>? explicitIncludes = null)
    {
        var paginatedResponse = await _repository.GetAsync(filter, orderBy, include, pagingParams);

        var items = _mapper
            .Map<IEnumerable<GenreReadDto>>(paginatedResponse.Items)
            .Select(genreDto =>
            {
                if (include is null)
                {
                    genreDto.GamesIds = null;
                }

                return genreDto;
            });


        return new PaginatedResponse<GenreReadDto>
        {
            Items = items,
            Pagination = paginatedResponse.Pagination
        };
    }
}
