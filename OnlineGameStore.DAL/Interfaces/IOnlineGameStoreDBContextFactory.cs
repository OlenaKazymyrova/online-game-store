using OnlineGameStore.DAL.DBContext;

namespace OnlineGameStore.DAL.Interfaces;

public interface IOnlineGameStoreDbContextFactory
{
    OnlineGameStoreDbContext CreateDbContext(string[] args);
}