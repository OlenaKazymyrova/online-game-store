using Microsoft.EntityFrameworkCore;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using AutoMapper;

namespace OnlineGameStore.BLL.Mappings;

public class PlatformService
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IMapper _mapper;

    public PlatformService(
        IPlatformRepository platformRepository,
        IGameRepository gameRepository,
        IMapper mapper)
    {
        _platformRepository = platformRepository;
        _gameRepository = gameRepository;
        _mapper = mapper;
    }

    public async Task<PlatformResponseDto> CreateAsync(PlatformDto createPlatformDto)
    {
        await ValidateGameIdsExist(createPlatformDto.GameIds);
        Platform newPlatform = _mapper.Map<Platform>(createPlatformDto);
        newPlatform.Id = Guid.NewGuid();
        await _platformRepository.AddAsync(newPlatform);
        return _mapper.Map<PlatformResponseDto>(newPlatform);
    }

    public async Task<IEnumerable<PlatformResponseDto>> GetAllAsync()
    {
        IEnumerable<Platform> platforms = await _platformRepository.GetAsync(include: src => src.Include(p => p.GamePlatforms));
        return  _mapper.Map<IEnumerable<PlatformResponseDto>>(platforms);
    }

    public async Task<PlatformResponseDto> GetById(Guid id)
    {

        Platform platform = (await _platformRepository.GetAsync(filter: p => p.Id == id, include: src => src.Include(p => p.GamePlatforms))).FirstOrDefault();
        if (platform == null)
        {
            throw new KeyNotFoundException($"Platform with {id} was not found");
        }

        return  _mapper.Map<PlatformResponseDto>(platform);
    }

    public async Task UpdateAsync(Guid id, PlatformDto updatePlatformDto)
    {
        Platform? existingPlatform = (await _platformRepository.GetAsync(
                filter: p => p.Id == id,
                include: src => src.Include(p => p.GamePlatforms)))
            .FirstOrDefault();

        if (existingPlatform == null)
        {
            throw new KeyNotFoundException($"Platform with id {id} was not found");
        }

        _mapper.Map(updatePlatformDto, existingPlatform);
        
        if (updatePlatformDto.GameIds != null)
        {
            await ValidateGameIdsExist(updatePlatformDto.GameIds);
            
            var existingGameIds = existingPlatform.GamePlatforms.Select(gp => gp.GameId).ToHashSet();
        
            var newGameIds = updatePlatformDto.GameIds.ToHashSet();

            
            var toRemove = existingPlatform.GamePlatforms
                .Where(gp => !newGameIds.Contains(gp.GameId))
                .ToList();
        
            foreach (var gamePlatform in toRemove)
            {
                existingPlatform.GamePlatforms.Remove(gamePlatform);
            }

           
            foreach (var gameId in newGameIds.Where(id => !existingGameIds.Contains(id)))
            {
                existingPlatform.GamePlatforms.Add(new GamePlatform { GameId = gameId });
            }
        }

        await _platformRepository.UpdateAsync(existingPlatform);
    }

    public async Task PatchAsync(Guid id, JsonPatchDocument<PlatformDto> patchDocument)
    {
        
        Platform? existingPlatform = (await _platformRepository.GetAsync(
                filter: p => p.Id == id,
                include: src => src.Include(p => p.GamePlatforms)))
            .FirstOrDefault();

        if (existingPlatform == null)
        {
            throw new KeyNotFoundException($"Platform with id {id} was not found");
        }

        
        var platformToPatch = _mapper.Map<PlatformDto>(existingPlatform);
        
        patchDocument.ApplyTo(platformToPatch);
    
        await ValidateGameIdsExist(platformToPatch.GameIds);
        
        _mapper.Map(platformToPatch, existingPlatform);
        
        if (platformToPatch.GameIds != null)
        {
            existingPlatform.GamePlatforms.Clear();
            foreach (var gameId in platformToPatch.GameIds)
            {
                existingPlatform.GamePlatforms.Add(new GamePlatform { GameId = gameId });
            }
        }

        await _platformRepository.UpdateAsync(existingPlatform);
    }
    
    

    public async Task DeleteAsync(Guid id)
    {
        Platform? platform = await _platformRepository.GetByIdAsync(id);
        if (platform == null) return;
        await _platformRepository.DeleteAsync(platform);

    }

    

    public async Task AddGamesToPlatform(Guid id,List<Guid> gameIds)
    {
        if (gameIds == null || gameIds.Count == 0) return;
        await ValidateGameIdsExist(gameIds);
        Platform? platform = (await _platformRepository.GetAsync(filter: p => p.Id == id, include: src => src.Include(p => p.GamePlatforms))).FirstOrDefault();
        if (platform == null)
        {
            throw new KeyNotFoundException($"Platform with {id} was not found");
        }

        HashSet<Guid> existingGameIds = platform.GamePlatforms.Select(gp => gp.GameId).ToHashSet();

        int addedGameIds = 0;

        foreach (Guid gameId in gameIds)
        {
            if(!existingGameIds.Contains(gameId))
            {
                platform.GamePlatforms.Add(new GamePlatform { GameId = gameId });
                addedGameIds++;
            }
        }

        if (addedGameIds > 0)
        {
            await _platformRepository.UpdateAsync(platform);
        }

    }

    public async Task RemoveGamesFromPlatform(Guid id, List<Guid> gameIds)
    {
        if (gameIds == null || gameIds.Count == 0) return;
        
        Platform? platform = (await _platformRepository.GetAsync(filter: p => p.Id == id, include: src => src.Include(p => p.GamePlatforms))).FirstOrDefault();
        if (platform == null)
        {
            throw new KeyNotFoundException($"Platform with {id} was not found");
        }

        int initialAmountGameIds = platform.GamePlatforms.Count;
        platform.GamePlatforms.RemoveAll(gp => gameIds.Contains(gp.GameId));
        if (initialAmountGameIds == platform.GamePlatforms.Count) return;
        await _platformRepository.UpdateAsync(platform);

    }

    public async Task  ReplaceGamesInPlatform(Guid id, List<Guid> gameIds)
    {
        if (gameIds == null || gameIds.Count == 0) return;
        
        Platform? platform = (await _platformRepository.GetAsync(filter: p => p.Id == id, include: src => src.Include(p => p.GamePlatforms))).FirstOrDefault();
        if (platform == null)
        {
            throw new KeyNotFoundException($"Platform with {id} was not found");
        }
        
        platform.GamePlatforms.Clear();
        foreach (Guid gameId in gameIds)
        {
            platform.GamePlatforms.Add(new GamePlatform{GameId = gameId});
        }

        await _platformRepository.UpdateAsync(platform);
    }



    

    private async Task ValidateGameIdsExist(List<Guid> gameIds)
    {
        if (gameIds == null || gameIds.Count == 0) return;
        
        var existingAmount = await _gameRepository.CountAsync(g => gameIds.Contains(g.Id));
        if (existingAmount != gameIds.Count)
        {
            throw new KeyNotFoundException("Mismatch in gameIds");
        }
        
    }
    
}