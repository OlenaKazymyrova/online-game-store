using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace OnlineGameStore.DAL.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{

    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
    );

    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TEntity?> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(Guid id);


}