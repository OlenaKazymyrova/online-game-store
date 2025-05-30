using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;

namespace OnlineGameStore.DAL.Tests.RepositoryCreators;

public class RepositoryCreator<T>
{
    public T Create()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new OnlineGameStoreDbContext(options);
        return (T)Activator.CreateInstance(typeof(T), context)!;
    }
}