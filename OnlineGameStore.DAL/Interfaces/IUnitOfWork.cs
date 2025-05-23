
namespace OnlineGameStore.DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
        ILicenseRepository Licenses { get; }
        IGameRepository Games { get; }
        //add other interfaces of repositories

        int Save();
    }
