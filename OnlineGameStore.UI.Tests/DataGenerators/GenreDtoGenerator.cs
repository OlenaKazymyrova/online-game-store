using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.UI.Tests.Interfaces;
using System.Security.Cryptography;

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
            parentGenreList.Add(new GenreDto
            {
                Id = Guid.NewGuid(),
                Name = $"Parent genre {RandomNumberGenerator.GetInt32(0, int.MaxValue)}",
                Description = $"Description {RandomNumberGenerator.GetInt32(0, int.MaxValue)}",
                ParentId = null
            });
        }
        for (var i = 0; i < childCount; i++)
        {
            childGenreList.Add(new GenreDto
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
