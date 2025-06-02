using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.UI.Tests.DataGenerators;

public class PlatformDtoDataGenerator : IDataGenerator<PlatformDto>
{
    public List<PlatformDto> Generate(int count)
    {
        var games = new List<PlatformDto>();
        for (int i = 0; i < count; i++)
        {
            games.Add(new PlatformDto
            {
                Id = Guid.NewGuid(),
                Name = $"Platform {i}"
            });
        }

        return games;
    }
}