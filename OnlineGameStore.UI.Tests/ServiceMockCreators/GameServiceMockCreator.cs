using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GameServiceMockCreator: ServiceMockCreator<Game,GameDto, IGameService>
{
    public GameServiceMockCreator(List<GameDto> data ) : base(data) {}
}