using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.BLL.Tests.DataGenerators;

public class UserEntityGenerator : IDataGenerator<User>
{
    public List<User> Generate(int count)
    {
        var list = new List<User>();

        for (var i = 0; i < count; i++)
        {
            list.Add(new User
            {
                Username = "User " + i,
                Email = $"user{i}@example.com",
                PasswordHash = "evojkfqodsfjeovnepaokd" + i
            });
        }

        return list;
    }
}