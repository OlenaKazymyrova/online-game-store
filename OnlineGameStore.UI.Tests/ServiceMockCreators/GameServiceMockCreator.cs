using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GameServiceMockCreator : ServiceMockCreator<Game, GameDto, GameDto, GameDto, IGameService>
{
    public GameServiceMockCreator(List<Game> data) : base(data) { }
}