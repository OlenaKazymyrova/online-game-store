using AutoMapper;
using OnlineGameStore.BLL.DTOs.Games;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class GameService : Service<Game, GameCreateDto, GameDto, GameDto, GameDetailedDto>, IGameService
{
    public GameService(IGameRepository repository, IMapper mapper)
        : base(repository, mapper) { }
}