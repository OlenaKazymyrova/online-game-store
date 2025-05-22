using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class GameService(IGameRepository repository) : IGameService
{
    public async Task<GameDto?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<GameDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<GameDto?> AddAsync(GameDto game)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(GameDto game)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}