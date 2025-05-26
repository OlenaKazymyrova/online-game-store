using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{

    Task<IEnumerable<TEntity>?> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
        );

    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TEntity?> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entityToDelete);
    Task DeleteByIdAsync(Guid id);


}