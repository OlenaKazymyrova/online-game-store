using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class PlatformRepository : Repository<Platform>, IPlatformRepository
{
    public PlatformRepository(OnlineGameStoreDbContext context) : base(context)
    { }

    public override async Task<Platform?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(platform => platform.Games)
            .FirstOrDefaultAsync(platform => platform.Id == id);
    }

    public async Task UpdateGameRefsAsync(Guid id, List<Game> games)
    {
        ArgumentNullException.ThrowIfNull(games);

        var entityToUpdate = await _dbSet.Include(platform => platform.Games).FirstOrDefaultAsync(platform => platform.Id == id);

        if (entityToUpdate is null)
        {
            throw new KeyNotFoundException($"Could not find the Platform with ID {id}");
        }

        entityToUpdate.Games = games;

        await _dbContext.SaveChangesAsync();
    }
}