using OnlineGameStore.DAL.Entities;
using System.Security.Cryptography;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.BLL.Tests.DataGenerators;

public class GenreEntityGenerator : IDataGenerator<Genre>
{
    public List<Genre> Generate(int count)
    {
        var parentGenreList = new List<Genre>();
        var childGenreList = new List<Genre>();

        int parentCount = (int)Math.Ceiling((double)count / 2);
        int childCount = count - parentCount;

        for (var i = 0; i < parentCount; i++)
        {
            var randomInt = RandomNumberGenerator.GetInt32(0, int.MaxValue);

            parentGenreList.Add(new Genre
            {
                Id = Guid.NewGuid(),
                Name = $"Parent genre {randomInt}",
                Description = $"Description {randomInt}",
                ParentGenre = null,
                ParentId = null
            });
        }

        for (var i = 0; i < childCount; i++)
        {
            var randomInt = RandomNumberGenerator.GetInt32(0, int.MaxValue);

            childGenreList.Add(new Genre
            {
                Id = Guid.NewGuid(),
                Name = $"Child genre {randomInt}",
                Description = $"Description {randomInt}",
                ParentId = parentGenreList[RandomNumberGenerator.GetInt32(0, parentGenreList.Count - 1)].Id,
            });
        }

        return [.. parentGenreList, .. childGenreList];
    }
}