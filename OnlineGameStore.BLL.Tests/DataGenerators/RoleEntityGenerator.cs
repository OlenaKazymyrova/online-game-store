using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.BLL.Tests.DataGenerators;

public class RoleEntityGenerator : IDataGenerator<Role>
{
    public List<Role> Generate(int count)
    {
        var list = new List<Role>();

        for (var i = 0; i < count; i++)
        {
            list.Add(new Role
            {
                Name = "Role " + i,
                Description = "Description for role " + i
            });
        }

        return list;
    }
}