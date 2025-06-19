using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.DTOs.Games;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GameServiceMockCreator : ServiceMockCreator<Game, GameCreateDto, GameDto, GameDto, GameDetailedDto, IGameService>
{
    public GameServiceMockCreator(List<Game> data) : base(data) { }
}