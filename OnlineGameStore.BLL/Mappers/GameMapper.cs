using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mappers;

public abstract class GameMapper : IMapper<Game, GameDto>
{
    public static GameDto Map(Game source)
    {
        return new GameDto
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            Publisher = source.Publisher,
            Genre = source.Genre,
            License = source.License
        };
    }

    public static Game Map(GameDto source)
    {
        return new Game
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            Publisher = source.Publisher,
            Genre = source.Genre,
            License = source.License
        };
    }

    public static IEnumerable<GameDto> Map(IEnumerable<Game> source)
    {
        return source.Select(Map);
    }

    public static IEnumerable<Game> Map(IEnumerable<GameDto> source)
    {
        return source.Select(Map);
    }
}