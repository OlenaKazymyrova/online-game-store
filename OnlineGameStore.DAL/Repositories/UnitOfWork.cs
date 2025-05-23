using OnlineGameStore.DAL;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Repositories;
using OnlineGameStore.DAL.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly OnlineGameStoreDbContext _dbContext;
    private IGameRepository? _gameRepository;
    private ILicenseRepository? _licenseRepository;
    //add other interfaces of repositories

    public UnitOfWork(OnlineGameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IGameRepository Games
    {
        get
        {
            if (_gameRepository == null)
            {
                _gameRepository = new GameRepository(_dbContext);
            }
            return _gameRepository;
        }
    }


    public ILicenseRepository Licenses
    {
        get
        {
            if (_licenseRepository == null)
            {
                _licenseRepository = new LicenseRepository(_dbContext);
            }
            return _licenseRepository;
        }
    }


    public int Save()
    {
        return _dbContext.SaveChanges();
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
        }
    }
}