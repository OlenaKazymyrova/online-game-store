
namespace OnlineGameStore.DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
        ILicenseRepository Licenses { get; }
        //add other interfaces of repositories

        int Save();
    }
