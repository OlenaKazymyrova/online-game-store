using Microsoft.AspNetCore.JsonPatch;
using OnlineGameStore.BLL.DTOs;
namespace OnlineGameStore.BLL.Interfaces;

public interface IPlatformService
{
    public Task<PlatformResponseDto> CreateAsync(PlatformDto platformDto);
    public Task DeleteAsync(Guid id);
    public Task<IEnumerable<PlatformResponseDto>> GetAllAsync();
    public Task<PlatformResponseDto> GetByIdAsync(Guid id);
    public Task UpdateAsync(Guid id, PlatformDto platformDto);
    Task PatchAsync(Guid id, JsonPatchDocument<PlatformDto> patchDocument);

    public Task AddGamesToPlatform(Guid Id, IEnumerable<Guid> gameIdsToAdd);
    public Task RemoveGamesFromPlatform(Guid Id, IEnumerable<Guid> gameIdsToRemove);
    public Task ReplaceGamesInPlatform(Guid Id, IEnumerable<Guid> newGameIds);
    

}