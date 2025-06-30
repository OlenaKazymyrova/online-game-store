namespace OnlineGameStore.DAL.Interfaces;

public interface IPlatformRepository : IRepository<Platform>
{
    public Task UpdateGameRefsAsync(Guid id, List<Game> games);
}