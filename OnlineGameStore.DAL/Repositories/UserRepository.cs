using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(OnlineGameStoreDbContext context) : base(context)
    {
    }
}