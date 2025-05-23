namespace OnlineGameStore.BLL.Tests.Interfaces;

public interface IDataGenerator<T>
{
    public List<T> Generate(int count);
}