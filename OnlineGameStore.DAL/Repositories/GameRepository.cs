using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class GameRepository : Repository<Game>, IGameRepository
{
    public GameRepository(OnlineGameStoreDbContext context) : base(context) { }
}