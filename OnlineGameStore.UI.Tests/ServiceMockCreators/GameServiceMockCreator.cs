using Moq;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.Interfaces;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GameServiceMockCreator : IServiceMockCreator<IGameService>
{
    private readonly List<GameDto> _data;

    public GameServiceMockCreator(List<GameDto> data)
    {
        _data = data;
    }

    public IGameService Create()
    {
        var service = new Mock<IGameService>();

        service.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => _data.FirstOrDefault(x => x.Id == id));

        service.Setup(x => x.GetAllAsync())
            .ReturnsAsync(_data);

        service.Setup(x => x.AddAsync(It.IsAny<GameDto>()))
            .ReturnsAsync((GameDto dto) =>
            {
                _data.Add(dto);
                return dto;
            });

        service.Setup(x => x.UpdateAsync(It.IsAny<GameDto>()))
            .ReturnsAsync((GameDto game) =>
            {
                var index = _data.FindIndex(x => x.Id == game.Id);
                if (index == -1) return false;
                _data[index] = game;
                return true;
            });

        service.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = _data.FindIndex(x => x.Id == id);
                if (index == -1) return false;
                _data.RemoveAt(index);
                return true;
            });

        return service.Object;
    }
}