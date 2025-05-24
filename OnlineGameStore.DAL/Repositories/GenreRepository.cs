using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.DBContext;


namespace OnlineGameStore.DAL.Repositories;

public class GenreRepository(OnlineGameStoreDbContext context) : IGenreRepository
{
    public async Task<Genre?> GetByIdAsync(Guid id)
    {
        return await context.Genres.FindAsync(id);
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await context.Genres.ToListAsync();
    }

    public async Task<Genre?> AddAsync(Genre entity)
    {
        if (entity.ParentId is not null
            && entity.ParentId != Guid.Empty
            && await context.Genres.FindAsync(entity.ParentId) is null)
        {
            return null;
        }

        try
        {
            await context.Genres.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding game: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(Genre entity)
    {

        if (await context.Genres.FindAsync(entity.Id) is not Genre genre)
        {
            return false;
        }

        if (entity.ParentId is Guid parentId
            && await context.Genres.FindAsync(parentId) is null)
        {
            return false;
        }

        context.Entry(genre).CurrentValues.SetValues(entity);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (await context.Genres.FindAsync(id) is not Genre genre)
        {
            return false;
        }

        context.Genres.Remove(genre);
        await context.SaveChangesAsync();
        return true;
    }



}
