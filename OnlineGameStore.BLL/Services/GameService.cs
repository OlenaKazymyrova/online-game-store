using System.Linq.Expressions;
using System.Reflection.Metadata;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.BLL.DTOs.Games;
using OnlineGameStore.BLL.Exceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.BLL.Services;

public class GameService : Service<Game, GameCreateDto, GameDto, GameDto, GameDetailedDto>, IGameService
{
    public GameService(IGameRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }

    public override async Task<PaginatedResponse<GameDetailedDto>> GetAsync(
        Expression<Func<Game, bool>>? filter = null,
        Func<IQueryable<Game>, IOrderedQueryable<Game>>? orderBy = null,
        Func<IQueryable<Game>, IIncludableQueryable<Game, object>>? include = null,
        PagingParams? pagingParams = null,
        HashSet<string>? explicitIncludes = null)
    {
        var paginatedResponse = await _repository.GetAsync(filter, orderBy, include, pagingParams);

        var includeGenres = explicitIncludes?.Contains("genres") == true;
        var includePlatforms = explicitIncludes?.Contains("platforms") == true;

        var mappedItems = _mapper.Map<IEnumerable<GameDetailedDto>>(
            paginatedResponse.Items,
            opts =>
            {
                opts.Items["IncludeGenres"] = includeGenres;
                opts.Items["IncludePlatforms"] = includePlatforms;
            });

        var itemsWithInclude = mappedItems.Select(game =>
        {
            if (!includeGenres)
                game.Genres = null;
            if (!includePlatforms)
                game.Platforms = null;
            return game;
        });

        return new PaginatedResponse<GameDetailedDto>
        {
            Items = itemsWithInclude,
            Pagination = paginatedResponse.Pagination
        };
    }

    public async Task UpdateGenreRefsAsync(Guid gameId, List<Guid> genresIds)
    {
        List<Genre> genreEntities;

        try
        {
            genreEntities = _mapper.Map<List<Genre>>(genresIds);
        }
        catch (AutoMapperMappingException e)
        {
            Exception? inner = e.InnerException;

            if (inner is AggregateException agg)
                inner = agg.Flatten().InnerExceptions
                    .FirstOrDefault(exception => exception is KeyNotFoundException) ?? agg;

            if (inner is KeyNotFoundException)
                throw new NotFoundException("One or more Genres were not found.");

            throw new InternalErrorException("An error occurred while mapping the GUID to the Genre entity.");
        }

        if (_repository is not IGameRepository gameRepo)
            throw new InvalidOperationException("Repository does not support genre updates");

        try
        {
            await gameRepo.UpdateGenreRefsAsync(gameId, genreEntities);
        }
        catch (ArgumentNullException e)
        {
            throw new ValidationException("The argument cannot be null", e);
        }
        catch (KeyNotFoundException e)
        {
            throw new NotFoundException("Entity not be found", e);
        }
        catch (DbUpdateException e)
        {
            throw new ConflictException("An error occured while updating", e);
        }
        catch (Exception e)
        {
            throw new InternalErrorException("An unexpected error occurred while updating the entity.", e);
        }
    }

    public async Task UpdatePlatformRefsAsync(Guid gameId, List<Guid> platformIds)
    {
        List<Platform> platformEntities;

        try
        {
            platformEntities = _mapper.Map<List<Platform>>(platformIds);
        }
        catch (AutoMapperMappingException e)
        {
            Exception? inner = e.InnerException;

            if (inner is AggregateException agg)
                inner = agg.Flatten().InnerExceptions
                    .FirstOrDefault(exception => exception is KeyNotFoundException) ?? agg;

            if (inner is KeyNotFoundException)
                throw new NotFoundException("One or more Platforms were not found.");

            throw new InternalErrorException("An error occurred while mapping the GUID to the Platform entity.");
        }

        if (_repository is not IGameRepository gameRepo)
            throw new InvalidOperationException("Repository does not support genre updates");

        try
        {
            await gameRepo.UpdatePlatformRefsAsync(gameId, platformEntities);
        }
        catch (ArgumentNullException e)
        {

            throw new ValidationException("The argument cannot be null", e);
        }
        catch (KeyNotFoundException e)
        {
            throw new NotFoundException("Entity cannot be found", e);
        }
        catch (DbUpdateException e)
        {
            throw new ConflictException("An error occured while updating", e);
        }
        catch (Exception e)
        {
            throw new InternalErrorException("An unexpected error occurred while updating the entity.", e);
        }

    }
}