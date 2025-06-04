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
                    && Data.Find(g => g.Id == genre.ParentId) is null)
                {
                    return null;
                }

                Data.Add(genre);
                return genre;
            });
    }

    protected override void SetupUpdate(Mock<IGenreRepository> mock)
    {
        mock.Setup(x => x.UpdateAsync(It.IsAny<Genre>()))
            .ReturnsAsync((Genre genre) =>
            {
                var index = Data.FindIndex(x => x.Id == genre.Id);
                if (index == -1)
                {
                    return false;
                }

                if (genre.ParentId is Guid parentId
                    && Data.Find(g => g.Id == genre.ParentId) is null)
                {
                    return false;
                }

                Data[index] = genre;
                return true;
            });
    }

    protected override void SetupDelete(Mock<IGenreRepository> mock)
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
                    item.ParentGenre = null;
                    item.ParentId = null;
                }

                return true;
            });
    }
}