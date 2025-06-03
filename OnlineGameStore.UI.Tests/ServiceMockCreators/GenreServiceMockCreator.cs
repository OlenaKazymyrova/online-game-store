using Moq;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Tests.Interfaces;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GenreServiceMockCreator(List<GenreDto> data) : IServiceMockCreator<IGenreService>
{

    public IGenreService Create()
    {
        var service = new Mock<IGenreService>();

        service.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => data.FirstOrDefault(x => x.Id == id));

        service.Setup(x => x.GetAllAsync()).ReturnsAsync(data);

        service.Setup(x => x.AddAsync(It.IsAny<GenreDto>()))
            .ReturnsAsync((GenreDto dto) =>
            {
                if (dto.ParentId is not null
                    && dto.ParentId != Guid.Empty
                    && data.Find(g => g.Id == dto.ParentId) is null)
                {
                    return null;
                }
                data.Add(dto);
                return dto;
            });

        service.Setup(x => x.UpdateAsync(It.IsAny<GenreDto>()))
            .ReturnsAsync((GenreDto dto) =>
            {
                var index = data.FindIndex(x => x.Id == dto.Id);
                if (index == -1)
                {
                    return false;
                }
                if (dto.ParentId is Guid parentId
                    && data.Find(g => g.Id == dto.ParentId) is null)
                {
                    return false;
                }

                data[index] = dto;
                return true;
            });


        service.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = data.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    return false;
                }

                data.RemoveAt(index);

                var parentRefToRemove = id;

                foreach (var item in data.Where(g => g.ParentId == parentRefToRemove))
                {
                    item.ParentId = null;
                }

                return true;
            });

        return service.Object;
    }

}
