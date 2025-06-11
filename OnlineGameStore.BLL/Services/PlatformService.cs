using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Services;

public class PlatformService : Service<Platform, PlatformCreateDto, PlatformDto, PlatformDto>, IPlatformService
{
    public PlatformService(IRepository<Platform> repository, IMapper mapper)
        : base(repository, mapper)
    {
    }

    public override async Task<PlatformDto?> AddAsync(PlatformCreateDto dto)
    {
        var entity = _mapper.Map<Platform>(dto);

        var existing = await _repository.GetAsync(
            filter: p => p.Name.ToLower() == entity.Name.ToLower()
        );

        if (existing.Items.Any()) return null;

        var addedEntity = await _repository.AddAsync(entity);

        return _mapper.Map<PlatformDto>(addedEntity);
    }

    public override async Task<bool> UpdateAsync(Guid id, PlatformCreateDto dto)
    {
        var entity = _mapper.Map<Platform>(dto);
        entity.Id = id;

        var existing = await _repository.GetAsync(
            filter: p => p.Name.ToLower() == entity.Name.ToLower()
        );

        if (existing.Items.Any()) return false;

        return await _repository.UpdateAsync(entity);
    }
}