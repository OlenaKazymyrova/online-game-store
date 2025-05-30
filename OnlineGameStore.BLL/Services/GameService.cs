using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _repository;
    private readonly IMapper _mapper;

    public GameService(IGameRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GameDto?> GetByIdAsync(Guid id)
    {
        var game = await _repository.GetByIdAsync(id);
        return game == null ? null : _mapper.Map<GameDto>(game);
    }

    public async Task<IEnumerable<GameDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<GameDto?> AddAsync(GameDto game)
    {
        var gameEntity = _mapper.Map<Game>(game);
        var createdGame = await _repository.AddAsync(gameEntity);
        return createdGame == null ? null : _mapper.Map<GameDto>(createdGame);
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