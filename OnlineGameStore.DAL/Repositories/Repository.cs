using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.DAL.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly OnlineGameStoreDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(OnlineGameStoreDbContext context)
    {
        _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _dbContext.Set<TEntity>();
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.AnyAsync(e =>
            EF.Property<Guid>(e, "Id") == id);
    }

    public virtual async Task<PaginatedResponse<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        PagingParams? pagingParams = null)
    {
        pagingParams ??= new PagingParams();

        IQueryable<TEntity> query = _dbSet;

        if (include != null)
        {
            query = include(query);
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        int skip = (pagingParams.Page - 1) * pagingParams.PageSize;
        query = query.Skip(skip).Take(pagingParams.PageSize);

        return new PaginatedResponse<TEntity>
        {
            Items = await query.ToListAsync(),
            Pagination = new PaginationMetadata
            {
                Page = pagingParams.Page,
                PageSize = pagingParams.PageSize,
                TotalItems = _dbSet.Count(),
                TotalPages = (int)Math.Ceiling((double)_dbSet.Count() / pagingParams.PageSize)
            }
        };
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<TEntity?> AddAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;

    }
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;

        int affected = await _dbContext.SaveChangesAsync();

        return affected > 0;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity is null)
        {
            throw new KeyNotFoundException("Entity not found.");
        }

        _dbSet.Remove(entity);

        return await _dbContext.SaveChangesAsync() > 0;
    }
}