using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.SharedLogic.Interfaces;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace OnlineGameStore.UI.Tests.DataGenerators;

public class GenreCreateDtoGenerator : IDataGenerator<GenreCreateDto>
{
    public List<GenreCreateDto> Generate(int count)
    {
        var resultList = new List<GenreCreateDto>();

        for (int i = 0; i < count; i++)
        {
            var randomInt = RandomNumberGenerator.GetInt32(0, int.MaxValue);

            var dto = new GenreCreateDto
            {
                Name = $"Genre {randomInt}",
                Description = $"Description {randomInt}",
                ParentId = null
            };

            resultList.Add(dto);
        }

        return resultList;
    }
}
