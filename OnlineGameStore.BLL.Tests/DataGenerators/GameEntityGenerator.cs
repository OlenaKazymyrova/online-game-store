using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.BLL.Tests.DataGenerators;

public class GameEntityGenerator : IDataGenerator<Game>
{
    public List<Game> Generate(int count)
    {
        var list = new List<Game>();

        for (var i = 0; i < count; i++)
        {
            list.Add(new Game
            {
                Id = Guid.NewGuid(),
                Name = $"Game {i}",
                Description = $"Description {i}",
                PublisherId = Guid.NewGuid(),
                LicenseId = Guid.NewGuid(),
                Price = 10m + i,
                ReleaseDate = DateTime.Today.AddDays(-i)
            });
        }

        return list;
    }
}