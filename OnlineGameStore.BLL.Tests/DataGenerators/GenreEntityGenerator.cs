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
            parentGenreList.Add(new Genre
            {
                Id = Guid.NewGuid(),
                Name = $"Parent genre {RandomNumberGenerator.GetInt32(0, int.MaxValue)}",
                Description = $"Description {RandomNumberGenerator.GetInt32(0, int.MaxValue)}",
                ParentGenre = null,
                ParentId = null
            });
        }

        for (var i = 0; i < childCount; i++)
        {
            childGenreList.Add(new Genre
            {
                Id = Guid.NewGuid(),
                Name = $"Child genre {RandomNumberGenerator.GetInt32(0, int.MaxValue)}",
                Description = $"Description {RandomNumberGenerator.GetInt32(0, int.MaxValue)}",
                ParentId = parentGenreList[RandomNumberGenerator.GetInt32(0, parentGenreList.Count - 1)].Id,
            });
        }

        return [.. parentGenreList, .. childGenreList];
    }
}