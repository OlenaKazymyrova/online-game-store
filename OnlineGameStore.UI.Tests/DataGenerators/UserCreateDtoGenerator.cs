using System.Security.Cryptography;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.UI.Tests.DataGenerators;

public class UserCreateDtoGenerator : IDataGenerator<UserCreateDto>
{
    public List<UserCreateDto> Generate(int count)
    {
        var resultList = new List<UserCreateDto>();

        for (int i = 0; i < count; i++)
        {
            var randomInt = RandomNumberGenerator.GetInt32(0, int.MaxValue);

            var dto = new UserCreateDto
            {
                Email = $"user{randomInt}@example.com",
                Username = $"user{randomInt}",
                Password = $"Password{randomInt}!"
            };

            resultList.Add(dto);
        }

        return resultList;
    }
}