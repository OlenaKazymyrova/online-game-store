using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.SharedLogic.Pagination;
using OnlineGameStore.BLL.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace OnlineGameStore.BLL.Services;

public class GenreService : Service<Genre, GenreCreateDto, GenreReadDto, GenreDto, GenreDetailedDto>, IGenreService
{
    public GenreService(IGenreRepository repository, IMapper mapper)
        : base(repository, mapper) { }

    public override async Task<PaginatedResponse<GenreDetailedDto>> GetAsync(
        Expression<Func<Genre, bool>>? filter = null,
        Func<IQueryable<Genre>, IOrderedQueryable<Genre>>? orderBy = null,
        Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>? include = null,
        PagingParams? pagingParams = null,
        HashSet<string>? explicitIncludes = null)
    {
        var paginatedResponse = await _repository.GetAsync(filter, orderBy, include, pagingParams);

        var items = _mapper
            .Map<IEnumerable<GenreDetailedDto>>(paginatedResponse.Items)
            .Select(genreDto =>
            {
                if (include is null)
                {
                    genreDto.Games = null;
                }

                return genreDto;
            });

        var itemsWithInclude = items.Select(genre =>
        {
            if (include is null)
                genre.Games = null;

            return genre;
        });


        return new PaginatedResponse<GenreDetailedDto>
        {
            Items = itemsWithInclude,
            Pagination = paginatedResponse.Pagination
        };
    }

    public async Task UpdateGameRefsAsync(Guid id, List<Guid> gameIds)
    {
        List<Game> gameEntities;

        try
        {
            gameEntities = _mapper.Map<List<Game>>(gameIds);
        }
        catch (AutoMapperMappingException e)
        {
            Exception? inner = e.InnerException;

            if (inner is AggregateException agg)
                inner = agg.Flatten().InnerExceptions
                    .FirstOrDefault(exception => exception is KeyNotFoundException) ?? agg;

            if (inner is KeyNotFoundException)
                throw new NotFoundException("One or more Games were not found.");

            throw new InternalErrorException("An error occurred while mapping the GUID to the Game entity.");
        }

        if (_repository is not IGenreRepository genreRepository)
            throw new InvalidOperationException("Repository does not support game updates.");

        try
        {
            await genreRepository.UpdateGameRefsAsync(id, gameEntities);
        }
        catch (ArgumentNullException e)
        {
            throw new ValidationException("The argument cannot be null.", e);
        }
        catch (KeyNotFoundException e)
        {
            throw new NotFoundException("Genre cannot be found.", e);
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new ConflictException("An error occured while updating.", e);
        }
        catch (Exception e)
        {
            throw new InternalErrorException("An unexpected error occurred while updating the entity.", e);
        }
    }
}
