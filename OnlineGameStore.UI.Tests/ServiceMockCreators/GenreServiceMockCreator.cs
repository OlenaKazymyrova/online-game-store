using Moq;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.UI.Tests.Interfaces;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GenreServiceMockCreator : ServiceMockCreator<Genre, GenreDto, IGenreService>
{
    public GenreServiceMockCreator(List<GenreDto> data) : base(data) { }

    protected override void SetupDelete(Mock<IGenreService> mock)
    {
        mock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = Data.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    return false;
                }

                Data.RemoveAt(index);

                var parentRefToRemove = id;

                foreach (var item in Data.Where(g => g.ParentId == parentRefToRemove))
                {
                    item.ParentId = null;
                }

                return true;
            });
    }
}