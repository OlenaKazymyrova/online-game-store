using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class GameService : Service<Game, GameDto>, IGameService
{
    public GameService(IGameRepository repository, IMapper mapper)
        : base(repository, mapper) { }
}