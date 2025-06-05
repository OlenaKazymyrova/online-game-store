namespace OnlineGameStore.SharedLogic.Interfaces;

public interface IDataGenerator<T>
{
    public List<T> Generate(int count);
}