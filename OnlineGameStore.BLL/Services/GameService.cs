using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.BLL.DTOs.Games;
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

        var items = mappedItems.Select(game =>
        {
            if (!includeGenres)
                game.GenreDtos = null;
            if (!includePlatforms)
                game.PlatformDtos = null;
            return game;
        });

        return new PaginatedResponse<GameDetailedDto>
        {
            Items = items,
            Pagination = paginatedResponse.Pagination
        };
    }
}