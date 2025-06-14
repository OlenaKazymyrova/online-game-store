using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.UI.Tests.DataGenerators;

public class GameDtoGenerator : IDataGenerator<GameDto>
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
                LicenseId = Guid.NewGuid(),
                Price = new decimal(i + 1) * 10,
                ReleaseDate = DateTime.Now.AddDays(-i)
            });
        }

        return games;
    }
}