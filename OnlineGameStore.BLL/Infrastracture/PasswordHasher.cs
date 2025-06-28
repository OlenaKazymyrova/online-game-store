using Microsoft.Win32.SafeHandles;

namespace OnlineGameStore.SharedLogic;

public class PasswordHasherer: IPasswordHasher
{
    public string Generate(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool Verify(string password, string hashedPasswod) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPasswod);
} 