using OnlineGameStore.BLL.DTOs.Games;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IGameService : IService<Game, GameCreateDto, GameDto, GameDto, GameDetailedDto>
{
    public Task UpdateGenresAsync(Guid gameId, List<Guid> genresIds);
    public Task UpdatePlatformsAsync(Guid gameId, List<Guid> platformIds);
}