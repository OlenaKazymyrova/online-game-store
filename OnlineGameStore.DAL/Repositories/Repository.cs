using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{

    protected readonly OnlineGameStoreDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(OnlineGameStoreDbContext context)
    {
        DbContext = context;
        DbSet = DbContext.Set<TEntity>();
    }

    //approved
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        IQueryable<TEntity> query = DbSet;

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

        return await query.ToListAsync();
    }


    //approved
    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }


    public virtual async Task<TEntity?> AddAsync(TEntity entity)
    {
        var entityTypeName = typeof(TEntity).Name;

        if (entity is null) return null;

        try
        {
            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding {entityTypeName}: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Error adding {entityTypeName}: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error adding {entityTypeName}: {ex.Message}");
        }

        return null;
    }



    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        var entityTypeName = typeof(TEntity).Name;

        DbSet.Attach(entity);
        DbContext.Entry(entity).State = EntityState.Modified;

        try
        {
            int affected = await DbContext.SaveChangesAsync();
            return affected > 0;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error updating {entityTypeName}: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Error updating {entityTypeName}: {ex.Message}");
        }

        return false;
    }


    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity == null) return false;

        try
        {
            DbSet.Remove(entity);
            return await DbContext.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error deleting game: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Error deleting game: {ex.Message}");
        }

        return false;
    }

}