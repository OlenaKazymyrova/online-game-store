using Microsoft.Win32.SafeHandles;

namespace OnlineGameStore.SharedLogic;

public interface  IPasswordHasher
{
    string Generate(string password);
    bool Verify(string password, string hashedPasswod);

}