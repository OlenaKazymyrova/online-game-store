using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class LicenseRepository : GenericRepository<License>, ILicenseRepository
{
    public LicenseRepository(IOnlineGameStoreDbContextFactory dbContextFactory) : base(dbContextFactory)

    {

    }
}