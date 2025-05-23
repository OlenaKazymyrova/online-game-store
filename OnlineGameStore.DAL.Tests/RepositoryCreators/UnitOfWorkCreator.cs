using OnlineGameStore.DAL.Repositories;

using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Repositories;

namespace OnlineGameStore.DAL.Tests.Creators;

public class UnitOfWorkCreator<T> where T : class
{
    private readonly OnlineGameStoreDbContext _context;

    public UnitOfWorkCreator()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new OnlineGameStoreDbContext(options);
    }

    public IUnitOfWork CreateUnitOfWork()
    {
        return new UnitOfWork(_context);
    }


}


