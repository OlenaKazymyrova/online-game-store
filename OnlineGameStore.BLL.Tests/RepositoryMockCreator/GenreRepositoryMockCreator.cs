using Moq;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public class GenreRepositoryMockCreator : RepositoryMockCreator<Genre, IGenreRepository>
{
    public GenreRepositoryMockCreator(List<Genre> data) : base(data) { }

    protected override void SetupAdd(Mock<IGenreRepository> mock)
    {
        mock.Setup(x => x.AddAsync(It.IsAny<Genre>()))
            .ReturnsAsync((Genre genre) =>
            {
                if (genre.ParentId is not null
                    && genre.ParentId != Guid.Empty
                    && _data.Find(g => g.Id == genre.ParentId) is null)
                {
                    throw new KeyNotFoundException("Parent genre not found.");
                }

                _data.Add(genre);
                return genre;
            });
    }

    protected override void SetupUpdate(Mock<IGenreRepository> mock)
    {
        mock.Setup(x => x.UpdateAsync(It.IsAny<Genre>()))
            .ReturnsAsync((Genre genre) =>
            {
                var index = _data.FindIndex(x => x.Id == genre.Id);
                if (index == -1)
                {
                    throw new KeyNotFoundException("Genre not found.");
                }

                if (genre.ParentId is Guid parentId
                    && _data.Find(g => g.Id == genre.ParentId) is null)
                {
                    return false;
                }

                _data[index] = genre;
                return true;
            });
    }

    protected override void SetupDelete(Mock<IGenreRepository> mock)
    {
        mock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = _data.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    throw new KeyNotFoundException("Genre not found.");
                }

                _data.RemoveAt(index);

                var parentRefToRemove = id;

                foreach (var item in _data.Where(g => g.ParentId == parentRefToRemove))
                {
                    item.ParentGenre = null;
                    item.ParentId = null;
                }

                return true;
            });
    }
}