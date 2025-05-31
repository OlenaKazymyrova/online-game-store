using Moq;
using OnlineGameStore.BLL.Tests.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public class GenreRepositoryMockCreator(List<Genre> data) : IRepositoryMockCreator<IGenreRepository>
{
    public IGenreRepository Create()
    {
        var repo = new Mock<IGenreRepository>();

        repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => data.FirstOrDefault(x => x.Id == id));

        repo.Setup(x => x.GetAllAsync())
            .ReturnsAsync(data);

        repo.Setup(x => x.AddAsync(It.IsAny<Genre>()))
            .ReturnsAsync((Genre genre) =>
            {
                if (genre.ParentId is not null
                    && genre.ParentId != Guid.Empty
                    && data.Find(g => g.Id == genre.ParentId) is null)
                {
                    return null;
                }
                data.Add(genre);
                return genre;
            });

        repo.Setup(x => x.UpdateAsync(It.IsAny<Genre>()))
            .ReturnsAsync((Genre genre) =>
            {
                var index = data.FindIndex(x => x.Id == genre.Id);
                if (index == -1)
                {
                    return false;
                }
                if (genre.ParentId is Guid parentId
                    && data.Find(g => g.Id == genre.ParentId) is null)
                {
                    return false;
                }

                data[index] = genre;
                return true;
            });

        repo.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
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
                    item.ParentGenre = null;
                    item.ParentId = null;
                }

                return true;
            });

        return repo.Object;

    }
}