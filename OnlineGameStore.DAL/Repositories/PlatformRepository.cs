using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class PlatformRepository : Repository<Platform>, IPlatformRepository
{
    public PlatformRepository(OnlineGameStoreDbContext context) : base(context)
    {
    }
}