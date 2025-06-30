using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Constants;
using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(OnlineGameStoreDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByNameAsync(string userName) =>
         await _dbSet.FirstOrDefaultAsync(u => u.Username == userName);
    
    public async Task<User?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    
    public async Task<User?> GetByRefreshTokenAsync(string refreshToken) =>
        await _dbSet.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    
}