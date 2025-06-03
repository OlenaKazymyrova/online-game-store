using Moq;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.Interfaces;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GameServiceMockCreator(List<GameDto> data) : IServiceMockCreator<IGameService>
{
    public IGameService Create()
    {
        var service = new Mock<IGameService>();

        service.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => data.FirstOrDefault(x => x.Id == id));

        service.Setup(x => x.GetAllAsync())
            .ReturnsAsync(data);

        service.Setup(x => x.AddAsync(It.IsAny<GameDto>()))
            .ReturnsAsync((GameDto dto) =>
            {
                data.Add(dto);
                return dto;
            });

        service.Setup(x => x.UpdateAsync(It.IsAny<GameDto>()))
            .ReturnsAsync((GameDto game) =>
            {
                var index = data.FindIndex(x => x.Id == game.Id);
                if (index == -1) return false;
                data[index] = game;
                return true;
            });

        service.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = data.FindIndex(x => x.Id == id);
                if (index == -1) return false;
                data.RemoveAt(index);
                return true;
            });

        return service.Object;
    }
}