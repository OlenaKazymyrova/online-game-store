using OnlineGameStore.BLL.Authentication.Interface;

namespace OnlineGameStore.BLL.Authentication.Services;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool Verify(string password, string hashedPasswod) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPasswod);
}