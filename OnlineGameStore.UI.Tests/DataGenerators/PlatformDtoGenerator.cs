using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.UI.Tests.DataGenerators;

public class PlatformDtoGenerator : IDataGenerator<PlatformDto>
{
    public List<PlatformDto> Generate(int count)
    {
        var platforms = new List<PlatformDto>();

        for (var i = 0; i < count; i++)
        {
            platforms.Add(new PlatformDto()
            {
                Name = $"Platform {i}"
            });
        }

        return platforms;
    }
}