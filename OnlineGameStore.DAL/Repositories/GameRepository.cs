using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class GameRepository : Repository<Game>, IGameRepository 
{
    public GameRepository(OnlineGameStoreDbContext context) : base(context) { }

    public override async Task<Game?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<Game>()
            .Include(game => game.Genres)
            .Include(game => game.Platforms)
            .FirstOrDefaultAsync(game => game.Id == id);
    }

    public async Task UpdateGenreRefsAsync(Guid id, List<Genre> genres)
    {
        ArgumentNullException.ThrowIfNull(genres);

        var entityToUpdate = await _dbSet.Include(game => game.Genres).FirstOrDefaultAsync(game => game.Id == id);

        if (entityToUpdate is null)
        {
            throw new KeyNotFoundException($"Could not find the Game with ID {id}");
        }

        entityToUpdate.Genres = genres;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error updating game: {ex.Message}, possibly Genre ID is invalid");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating game: {ex.Message}");
            throw;
        }
    }

    public async Task UpdatePlatformRefsAsync(Guid id, List<Platform> platforms)
    {
        ArgumentNullException.ThrowIfNull(platforms);

        var entityToUpdate = await _dbSet.Include(game => game.Platforms).FirstOrDefaultAsync(game => game.Id == id);

        if (entityToUpdate is null)
        {
            throw new KeyNotFoundException($"Could not find the Game with ID {id}");
        }

        entityToUpdate.Platforms = platforms;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error updating game: {ex.Message}, possibly Platform ID is not found");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating game: {ex.Message}");
            throw;
        }
    }
}
