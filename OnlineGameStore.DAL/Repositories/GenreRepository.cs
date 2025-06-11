using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.DBContext;

namespace OnlineGameStore.DAL.Repositories;

public class GenreRepository(OnlineGameStoreDbContext context) : Repository<Genre>(context), IGenreRepository
{
    public override async Task<Genre?> AddAsync(Genre entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var isParentGenreValid = await IsParentGenreValidAsync(entity.Id, entity.ParentId);

        if (!isParentGenreValid)
        {
            return null;
        }

        try
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding genre: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }

    public override async Task<bool> UpdateAsync(Genre entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var existingGenre = await _dbSet.FindAsync(entity.Id);

        if (existingGenre is null)
        {
            return false;
        }

        var isParentGenreValid = await IsParentGenreValidAsync(entity.Id, entity.ParentId);

        if (!isParentGenreValid)
        {
            return false;
        }

        _dbContext.Entry(existingGenre).CurrentValues.SetValues(entity);

        try
        {
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error updating genre: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }

    public override async Task<bool> DeleteAsync(Guid id)
    {
        var existingGenre = await _dbSet.FindAsync(id);

        if (existingGenre is null)
        {
            return false;
        }

        if (existingGenre.ParentId is null)
        {
            var childGenres = _dbSet.Where(g => g.ParentId == existingGenre.Id).ToList();
            _dbSet.RemoveRange(childGenres);
        }

        _dbSet.Remove(existingGenre);

        try
        {
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error deleting genre: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }

    private async Task<bool> IsParentGenreValidAsync(Guid genreId, Guid? parentId)
    {
        if (parentId is null)
            return true;

        if (parentId == Guid.Empty || genreId == parentId)
            return false;

        return await _dbContext.Genres.FindAsync(parentId) is not null;
    }
}