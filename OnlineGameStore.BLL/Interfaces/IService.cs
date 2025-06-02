using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace OnlineGameStore.BLL.Interfaces;

public interface IService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    Task<TDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<TDto>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
    Task<TDto?> AddAsync(TDto dto);
    Task<bool> UpdateAsync(TDto dto);
    Task<bool> DeleteAsync(Guid id);

}