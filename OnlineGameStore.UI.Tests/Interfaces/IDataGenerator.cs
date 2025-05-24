namespace OnlineGameStore.UI.Tests.Interfaces;

public interface IDataGenerator<T>
{
    public List<T> Generate(int count);
}