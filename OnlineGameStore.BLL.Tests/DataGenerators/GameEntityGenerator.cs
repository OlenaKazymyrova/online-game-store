using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.BLL.Tests.DataGenerators;

public class GameEntityGenerator : IDataGenerator<Game>
{
    public List<Game> Generate(int count)
    {
        var list = new List<Game>();
        var genreGen = new GenreEntityGenerator();
        var platformGen = new PlatformEntityGenerator();

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

        // #########################
        // Generate navigation properties for some entities
        // #########################
        for (var i = 0; i < count / 2; i++)
        {
            list[i].Genres.Add(genreGen.Generate(1).First());
            list[i].Platforms.Add(platformGen.Generate(1).First());
        }

        return list;
    }
}