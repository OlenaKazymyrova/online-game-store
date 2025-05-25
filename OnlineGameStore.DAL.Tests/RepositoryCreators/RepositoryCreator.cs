using OnlineGameStore.DAL.Repositories;

namespace OnlineGameStore.DAL.Tests.Creators;

using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Repositories;


public class RepositoryCreator<T> where T : class
{
    private readonly OnlineGameStoreDbContext _context;

    public RepositoryCreator()
    {
        var options = new DbContextOptionsBuilder<OnlineGameStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new OnlineGameStoreDbContext(options);
    }

    public T CreateRepository()
    {
        if (typeof(T) == typeof(IGameRepository))
            return (T)(object)new GameRepository(_context);
        if (typeof(T) == typeof(ILicenseRepository))
            return (T)(object)new LicenseRepository(_context);

        throw new InvalidOperationException($"Unsupported repository type: {typeof(T)}");
    }



}