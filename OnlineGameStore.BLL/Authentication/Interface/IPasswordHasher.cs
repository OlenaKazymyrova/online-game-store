namespace OnlineGameStore.BLL.Authentication.Interface;

public interface IPasswordHasher
{
    string Generate(string password);
    bool Verify(string password, string hashedPasswod);
}