using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Interfaces;

public interface IGameRepository : IRepository<Game>
{
    public Task UpdateGenreRefsAsync(Guid id, List<Genre> genres);

    public Task UpdatePlatformRefsAsync(Guid id, List<Platform> platforms);
}