using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Interfaces;


namespace OnlineGameStore.SharedLogic.DataGenerators.DataEntityGenerators;

public class PlatformGenerator : IDataGenerator<Platform>
{
    public List<Platform> Generate(int count)
    {
        var list = new List<Platform>();

        for (var i = 0; i < count; i++)
        {
            list.Add(new Platform
            {
                Id = Guid.NewGuid(),
                Name = $"Platform {i}"
            });
        }

        return list;
    }
}