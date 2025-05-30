using Moq;
using OnlineGameStore.BLL.Tests.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public class GameRepositoryMockCreator(List<Game> data) : IRepositoryMockCreator<IGameRepository>
{
    public IGameRepository Create()
    {
        var repo = new Mock<IGameRepository>();

        repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => data.FirstOrDefault(x => x.Id == id));

        repo.Setup(x => x.GetAllAsync())
            .ReturnsAsync(data);

        repo.Setup(x => x.AddAsync(It.IsAny<Game>()))
            .ReturnsAsync((Game game) =>
            {
                data.Add(game);
                return game;
            });

        repo.Setup(x => x.UpdateAsync(It.IsAny<Game>()))
            .ReturnsAsync((Game game) =>
            {
                var index = data.FindIndex(x => x.Id == game.Id);
                if (index == -1) return false;
                data[index] = game;
                return true;
            });

        repo.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = data.FindIndex(g => g.Id == id);
                if (index == -1) return false;
                data.RemoveAt(index);
                return true;
            });

        return repo.Object;
    }
}