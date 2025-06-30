using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Infrastracture;

public interface IJwtProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}