using OnlineGameStore.BLL.Tests.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Tests.DataGenerators;

public class GameDataGenerator : IDataGenerator<Game>
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
                Publisher = Guid.NewGuid(),
                Genre = Guid.NewGuid(),
                License = Guid.NewGuid(),
                Price = new decimal(i + 1) * 10,
                ReleaseDate = DateTime.Now.AddDays(-i)
            });
        }

        return list;
    }
}