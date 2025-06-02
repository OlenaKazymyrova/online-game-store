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
        if (!await IfGenreParentRelationConsistentAsync(entity))
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
        if (entity is null
            || await _context.Genres.FindAsync(entity.Id) is not Genre genre)
        {
            return false;
        }

        if (entity.ParentId is Guid parentId
            && await _context.Genres.FindAsync(parentId) is null)
        {
            return false;
        }

        _context.Entry(genre).CurrentValues.SetValues(entity);

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
        if (await _context.Genres.FindAsync(id) is not Genre genre)
        {
            return false;
        }

        _context.Genres.Remove(genre);

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

    private async Task<bool> IfGenreParentRelationConsistentAsync(Genre entity)
    {
        return entity is not null
            && ((entity.ParentId is not null
            && entity.ParentId != Guid.Empty
            && await _context.Genres.FindAsync(entity.ParentId) is not null)
            || entity.ParentId is null);
    }

}
