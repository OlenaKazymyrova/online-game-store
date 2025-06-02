using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.DBContext;

namespace OnlineGameStore.DAL.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly OnlineGameStoreDbContext _context;

    public GenreRepository(OnlineGameStoreDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Genre?> GetByIdAsync(Guid id)
    {
        return await _context.Genres.FindAsync(id);
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<Genre?> AddAsync(Genre entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var isParentGenreValid = await IsParentGenreValidAsync(entity.ParentId);

        if (!isParentGenreValid)
        {
            return null;
        }

        try
        {
            await _context.Genres.AddAsync(entity);
            await _context.SaveChangesAsync();
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

    public async Task<bool> UpdateAsync(Genre entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var existingGenre = await _context.Genres.FindAsync(entity.Id);

        if (existingGenre is null)
        {
            return false;
        }

        var isParentGenreValid = await IsParentGenreValidAsync(entity.ParentId);

        if (!isParentGenreValid)
        {
            return false;
        }

        _context.Entry(existingGenre).CurrentValues.SetValues(entity);

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error updating genre: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existingGenre = await _context.Genres.FindAsync(id);

        if (existingGenre is null)
        {
            return false;
        }

        _context.Genres.Remove(existingGenre);

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error deleting genre: {ex.Message}");
            throw;
        }
    }

    private async Task<bool> IsParentGenreValidAsync(Guid? parentId)
    {
        if (parentId is null)
            return true;
        if (parentId == Guid.Empty)
            return false;

        return await _context.Genres.FindAsync(parentId) is not null;
    }
}
