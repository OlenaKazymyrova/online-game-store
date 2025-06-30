using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Authentication.Interface;

public interface IJwtProvider
{ 
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}