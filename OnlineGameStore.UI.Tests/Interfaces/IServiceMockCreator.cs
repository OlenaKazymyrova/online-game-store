namespace OnlineGameStore.UI.Tests.Interfaces;

public interface IServiceMockCreator<out TService>
{
    TService Create();
}