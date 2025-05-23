using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Interaces;

public interface IGenericRepository<TEntity> where TEntity : class
{

    Task<IEnumerable<TEntity>> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
        );

    Task<TEntity?> GetById(Guid id); 
    Task<TEntity?> Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entityToDelete);
    void DeleteById(Guid id);

    
}