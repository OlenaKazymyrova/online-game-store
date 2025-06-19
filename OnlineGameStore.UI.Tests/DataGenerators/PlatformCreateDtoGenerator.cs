using OnlineGameStore.BLL.DTOs.Platforms;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.UI.Tests.DataGenerators;

public class PlatformCreateDtoGenerator : IDataGenerator<PlatformCreateDto>
{
    public List<PlatformCreateDto> Generate(int count)
    {
        var platforms = new List<PlatformCreateDto>();

        for (var i = 0; i < count; i++)
        {
            platforms.Add(new PlatformCreateDto()
            {
                Name = $"New Platform {i}"
            });
        }

        return platforms;
    }
}