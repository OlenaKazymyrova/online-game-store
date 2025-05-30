using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.UI.Tests.Interfaces;

namespace OnlineGameStore.UI.Tests.DataGenerators;

public class GameDtoDataGenerator : IDataGenerator<GameDto>
{
    public List<GameDto> Generate(int count)
    {
        var games = new List<GameDto>();
        for (int i = 0; i < count; i++)
        {
            games.Add(new GameDto
            {
                Id = Guid.NewGuid(),
                Name = $"Game {i}",
                Description = $"Description for Game {i}",
                PublisherId = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                LicenseId = Guid.NewGuid()
            });
        }

        return games;
    }
}