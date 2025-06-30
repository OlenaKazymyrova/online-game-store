using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.DAL.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByNameAsync(string userName);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
}