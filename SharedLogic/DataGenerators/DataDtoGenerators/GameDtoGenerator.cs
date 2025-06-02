using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.SharedLogic.DataGenerators.DataEntityGenerators;

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
                LicenseId = Guid.NewGuid(),
                Price = 10m + i,
                ReleaseDate = DateTime.Today.AddDays(-i)
            });
        }

        return games;
    }
}