using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class PlatformRepository : GenericRepository<Platform>, IPlatformRepository
{
    public PlatformRepository(IOnlineGameStoreDbContextFactory dbContextFactory) : base(dbContextFactory)
    {

    }
}