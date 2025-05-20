using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Tests;

public interface IRepositoryCreator<T1, T2> where T1 : IRepository<T2>
{
    public static T1 Create()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new OnlineGameStoreDbContext(options);
        return (T1)Activator.CreateInstance(typeof(T1), context)!;
    }
}