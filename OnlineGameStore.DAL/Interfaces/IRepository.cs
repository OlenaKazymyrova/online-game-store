using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.DAL.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<PaginatedResponse<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        PagingParams? pagingParams = null
    );

    Task<bool> ExistsAsync(Guid id);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TEntity?> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(Guid id);
}