using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Services;

public class PlatformService : Service<Platform, PlatformCreateDto, PlatformDto, PlatformDto>, IPlatformService
{
    public PlatformService(IPlatformRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }

    public override async Task<PlatformDto?> AddAsync(PlatformCreateDto? dto)
    {
        if (dto is null)
            return null;
        
        Platform entity;
        try
        {
            entity = _mapper.Map<Platform>(dto);
        }
        catch (AutoMapperMappingException e)
        {
            Exception? inner = e.InnerException;

            if (inner is AggregateException agg)
                inner = agg.Flatten().InnerExceptions
                    .FirstOrDefault(ex => ex is KeyNotFoundException) ?? agg;

            if (inner is KeyNotFoundException)
                return null;

            throw;
        }

        if (await NameExistsAsync(entity.Name))
            throw new ValidationException("Platform name already exists.");

        var addedEntity = await _repository.AddAsync(entity);
        return addedEntity == null ? null : _mapper.Map<PlatformDto>(addedEntity);
    }

    public override async Task<bool> UpdateAsync(Guid id, PlatformCreateDto? dto)
    {
        if (dto is null)
            return false;

        var entity = _mapper.Map<Platform>(dto);
        entity.Id = id;

        if (await NameExistsAsync(entity.Name, id))
            throw new ValidationException("Platform name already exists.");

        return await _repository.UpdateAsync(entity);
    }

    private async Task<bool> NameExistsAsync(string name, Guid? excludeId = null)
    {
        var existing = await _repository.GetAsync(
            filter: p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase) && p.Id != excludeId);

        return existing.Items.Any();
    }
}