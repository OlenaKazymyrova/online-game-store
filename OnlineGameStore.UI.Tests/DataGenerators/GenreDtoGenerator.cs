using OnlineGameStore.BLL.DTOs;
using System.Security.Cryptography;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.UI.Tests.DataGenerators;

public class GenreDtoGenerator : IDataGenerator<GenreDto>
{
    public List<GenreDto> Generate(int count)
    {
        var parentGenreList = new List<GenreDto>();
        var childGenreList = new List<GenreDto>();

        int parentCount = (int)Math.Ceiling((double)count / 2);
        int childCount = count - parentCount;

        for (var i = 0; i < parentCount; i++)
        {
            var randomInt = RandomNumberGenerator.GetInt32(0, int.MaxValue);

            parentGenreList.Add(new GenreDto
            {
                Id = Guid.NewGuid(),
                Name = $"Parent genre {randomInt}",
                Description = $"Description {randomInt}",
                ParentId = null
            });
        }

        for (var i = 0; i < childCount; i++)
        {
            var randomInt = RandomNumberGenerator.GetInt32(0, int.MaxValue);

            childGenreList.Add(new GenreDto
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