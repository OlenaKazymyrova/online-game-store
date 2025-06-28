using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Interfaces;

public interface IGenreRepository : IRepository<Genre>
{
    public Task UpdateGameRefsAsync(Guid id, List<Game> games);
}
