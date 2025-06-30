namespace OnlineGameStore.BLL.Infrastracture;

public interface  IPasswordHasher
{
    string Generate(string password);
    bool Verify(string password, string hashedPasswod);

}