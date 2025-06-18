using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.BLL.DTOs.Games;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;
using System.Linq.Expressions;

namespace OnlineGameStore.BLL.Services;

public class GameService : Service<Game, GameCreateDto, GameDto, GameDto>, IGameService
{
    public GameService(IGameRepository repository, IMapper mapper)
        : base(repository, mapper) { }


    public virtual async Task<PaginatedResponse<>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        PagingParams? pagingParams = null)
    {
        var paginatedResponse = await _repository.GetAsync(filter, orderBy, include, pagingParams);

        return new PaginatedResponse<TReadDto>
        {
            Items = _mapper.Map<IEnumerable<TReadDto>>(paginatedResponse.Items),
            Pagination = paginatedResponse.Pagination
        };
    }
}