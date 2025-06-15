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
}
