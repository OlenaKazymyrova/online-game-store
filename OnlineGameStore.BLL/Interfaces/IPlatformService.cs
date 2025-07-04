using OnlineGameStore.BLL.DTOs.Platforms;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IPlatformService : IService<Platform, PlatformCreateDto, PlatformDto, PlatformDto, PlatformDetailedDto>
{
    public Task UpdateGameRefsAsync(Guid platformId, List<Guid> platformIds);
}