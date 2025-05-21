using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class GameRepository(OnlineGameStoreDbContext context) : IGameRepository
{
    public async Task<Game?> GetByIdAsync(int id)
    {
        return await context.Games.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<List<Game>> GetAllAsync()
    {
        return await context.Games.ToListAsync();
    }

    public async Task<Game?> AddAsync(Game entity)
    {
        try
        {
            await context.Games.AddAsync(entity);
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

    public async Task<bool> UpdateAsync(Game entity)
    {
        var game = await context.Games.FirstOrDefaultAsync(g => g.Id == entity.Id);
        if (game == null)
        {
            return false;
        }

        game.Name = entity.Name;
        game.Description = entity.Description;
        game.Publisher = entity.Publisher;
        game.Genre = entity.Genre;
        game.License = entity.License;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var game = await context.Games.FirstOrDefaultAsync(g => g.Id == id);
        if (game == null)
        {
            return false;
        }

        context.Games.Remove(game);
        await context.SaveChangesAsync();
        return true;
    }
}