namespace OnlineGameStore.BLL.Tests.Interfaces;

public interface IRepositoryMockCreator<out TRepository>
{
    TRepository Create();
}