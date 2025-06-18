using Moq;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GenreServiceMockCreator : ServiceMockCreator<Genre, GenreCreateDto, GenreReadDto, GenreDto, IGenreService>
{
    public GenreServiceMockCreator(List<Genre> data) : base(data) { }

    protected override void SetupDelete(Mock<IGenreService> mock)
    {
        mock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = _data.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    return false;
                }

                _data.RemoveAt(index);

                var parentRefToRemove = id;

                _data.RemoveAll(g => g.ParentId == parentRefToRemove);

                return true;
            });
    }
}