// UnitOfWorkDemo.Infrastructure.Repositories

using OnlineGameStore.DAL;
using OnlineGameStore.DAL.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly OnlineGameStoreDbContext _dbContext;
    
    public ILicenseRepository Licenses { get; }
    //add other interfaces of repositories

    public UnitOfWork(
        OnlineGameStoreDbContext dbContext,
        ILicenseRepository licenseRepository
      //add other interfaces of repositories
      ) 
    {
        _dbContext = dbContext;
        Licenses = licenseRepository;
        //add other interfaces of repositories
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