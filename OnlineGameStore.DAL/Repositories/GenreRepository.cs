using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.DBContext;

namespace OnlineGameStore.DAL.Repositories;

public class GenreRepository : Repository<Genre>, IGenreRepository
{
    public GenreRepository(OnlineGameStoreDbContext context) : base(context) { }
    public override async Task<Genre?> AddAsync(Genre entity)
    {
        if (entity.ParentId is not null
            && entity.ParentId != Guid.Empty
            && await DbSet.FindAsync(entity.ParentId) is null)
        {
            return null;
        }

        try
        {
            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding genre: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Error adding genre: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }

        return null;
    }

    public override async Task<bool> UpdateAsync(Genre entity)
    {

        if (await DbSet.FindAsync(entity.Id) is not Genre genre)
        {
            return false;
        }

        if (entity.ParentId is Guid parentId
            && await DbSet.FindAsync(parentId) is null)
        {
            return false;
        }

        DbContext.Entry(genre).CurrentValues.SetValues(entity);

        try
        {
            await DbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error updating genre: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Error updating genre: {ex.Message}");
        }

        return false;
    }
}

