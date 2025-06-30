using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.BLL.Tests.DataGenerators;

public class PlatformEntityGenerator : IDataGenerator<Platform>
{
    public List<Platform> Generate(int count)
    {
        var list = new List<Platform>();

        for (var i = 0; i < count; i++)
        {
            list.Add(new Platform()
            {
                Name = $"Platform {i}"
            });
        }

        return list;
    }
}