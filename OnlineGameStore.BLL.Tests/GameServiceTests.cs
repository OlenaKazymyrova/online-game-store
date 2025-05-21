using OnlineGameStore.BLL.Services;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.BLL.Tests.RepositoryMockCreator;

namespace OnlineGameStore.BLL.Tests;

public class GameServiceTests
{
    private const int EntityCount = 100;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        var gen = new GameDataGenerator();

        var data = gen.Generate(EntityCount);
        var repMock = new GameRepositoryMockCreator(data);

        var mockRepository = repMock.Create();

        _gameService = new GameService(mockRepository);
    }
}