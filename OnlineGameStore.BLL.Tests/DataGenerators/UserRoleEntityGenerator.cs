using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Tests.DataGenerators;

public class UserRoleEntityGenerator
{
    public List<UserRole> Generate(int count, List<User> users, List<Role> roles)
    {
        var list = new List<UserRole>();

        for (var i = 0; i < count; i++)
        {
            var userIndex = Random.Shared.Next(users.Count);
            var roleIndex = Random.Shared.Next(roles.Count);

            list.Add(new UserRole
            {
                UserId = users[userIndex].Id,
                RoleId = roles[roleIndex].Id
            });
        }

        return list;
    }
}