using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class GameRepository : GenericRepository<Game>, IGameRepository
{

    public GameRepository(IOnlineGameStoreDbContextFactory dbContextFactory) : base(dbContextFactory)

    {

    }
}