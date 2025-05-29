using OnlineGameStore.BLL.Tests.Interfaces;
using OnlineGameStore.DAL.Entities;
using System.Security.Cryptography;

namespace OnlineGameStore.BLL.Tests.DataGenerators;


public class GenreDataGenerator : IDataGenerator<Genre>
{
    public List<Genre> Generate(int count)
    {
        var parentGenreList = new List<Genre>();
        var childGenreList = new List<Genre>();

        int parentCount = count / 2;
        int childCount = count - parentCount;


        for (var i = 0; i < parentCount; i++)
        {
            parentGenreList.Add(new Genre
            {
                Id = Guid.NewGuid(),
                Name = $"Parent genre {i}",
                Description = $"Description {i}",
                ParentGenre = null,
                ParentId = null
            });
        }
        for (var i = 0; i < childCount; i++)
        {
            childGenreList.Add(new Genre
            {
                Id = Guid.NewGuid(),
                Name = $"Child genre {i}",
                Description = $"Description {i}",
                ParentId = parentGenreList[RandomNumberGenerator.GetInt32(0, parentCount - 1)].Id,
            });
        }

        return [.. parentGenreList, .. childGenreList];
    }
}
