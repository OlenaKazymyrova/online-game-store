using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(OnlineGameStoreDbContext context) : base(context)
    {
    }
}