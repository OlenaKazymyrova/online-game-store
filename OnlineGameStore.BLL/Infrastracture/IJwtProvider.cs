using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}