namespace OnlineGameStore.UI.Tests.Interfaces;

public interface IServiceMockCreator<out TRepository>
{
    TRepository Create();
}