using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public class GameRepositoryMockCreator : RepositoryMockCreator<Game, IGameRepository>
{
    public GameRepositoryMockCreator(List<Game> data) : base(data) { }
}