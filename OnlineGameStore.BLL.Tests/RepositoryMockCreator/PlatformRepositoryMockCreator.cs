using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public class PlatformRepositoryMockCreator : RepositoryMockCreator<Platform, IPlatformRepository>
{
    public PlatformRepositoryMockCreator(List<Platform> data) : base(data) { }
}