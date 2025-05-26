using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class LicenseRepository : GenericRepository<License>, ILicenseRepository
{
<<<<<<< HEAD
    public LicenseRepository(OnlineGameStoreDbContextFactory dbContextFactory) : base(dbContextFactory)
=======
    public LicenseRepository(IOnlineGameStoreDbContextFactory dbContextFactory) : base(dbContextFactory)
>>>>>>> feature/license
    {

    }
}