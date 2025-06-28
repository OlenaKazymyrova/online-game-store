using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}