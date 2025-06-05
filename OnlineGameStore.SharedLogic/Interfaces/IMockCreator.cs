namespace OnlineGameStore.SharedLogic.Interfaces;

public interface IMockCreator<out T>
{
    T Create();
}