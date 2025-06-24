using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
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
}