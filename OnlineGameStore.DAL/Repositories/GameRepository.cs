using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class GameRepository(OnlineGameStoreDbContext context) : IGameRepository
{
    public async Task<Game?> GetByIdAsync(Guid id)
    {
        return await context.Games.FindAsync(id);
    }

    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await context.Games.ToListAsync();
    }

    public async Task<Game?> AddAsync(Game entity)
    {
        if (entity is null)
        {
            return null;
        }

        try
        {
            await context.Games.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding a game: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Error adding game: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }

        return null;
    }

    public async Task<bool> UpdateAsync(Game entity)
    {
        if (entity is null)
        {
            return false;
        }

        var game = await context.Games.FindAsync(entity.Id);

        if (game == null)
        {
            return false;
        }

        game.Name = entity.Name;
        game.Description = entity.Description;
        game.Publisher = entity.Publisher;
        game.Genre = entity.Genre;
        game.License = entity.License;

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error updating game: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Error updating game: {ex.Message}");
        }

        return false;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var game = await context.Games.FindAsync(id);
        if (game == null)
        {
            return false;
        }

        context.Games.Remove(game);

        try
        {
            await context.SaveChangesAsync();
            return true;
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