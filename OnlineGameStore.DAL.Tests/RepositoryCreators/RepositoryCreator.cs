using Microsoft.EntityFrameworkCore;

namespace OnlineGameStore.DAL.Tests;

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