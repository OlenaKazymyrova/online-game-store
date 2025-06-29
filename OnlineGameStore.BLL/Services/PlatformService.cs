using AutoMapper;
using OnlineGameStore.BLL.DTOs.Platforms;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.BLL.Exceptions;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.BLL.Services;

public class PlatformService : Service<Platform, PlatformCreateDto, PlatformDto, PlatformDto, PlatformDetailedDto>,
    IPlatformService
{
    public PlatformService(IPlatformRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }

    public override async Task<PaginatedResponse<PlatformDetailedDto>> GetAsync(
        Expression<Func<Platform, bool>>? filter = null,
        Func<IQueryable<Platform>, IOrderedQueryable<Platform>>? orderBy = null,
        Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>? include = null,
        PagingParams? pagingParams = null,
        HashSet<string>? explicitIncludes = null)
    {
        var paginatedResponse = await _repository.GetAsync(filter, orderBy, include, pagingParams);

        var mappedItems = _mapper.Map<IEnumerable<PlatformDetailedDto>>(paginatedResponse.Items);

        var itemsWithInclude = mappedItems.Select(platform =>
        {
            if (include is null)
                platform.Games = null;

            return platform;
        });

        return new PaginatedResponse<PlatformDetailedDto>
        {
            Items = itemsWithInclude,
            Pagination = paginatedResponse.Pagination
        };
    }

    public override async Task<PlatformDto> AddAsync(PlatformCreateDto? dto)
    {
        if (dto is null)
            throw new ValidationException("PlatformCreateDto is required for create.");

        var entity = TryMap(dto);

        if (await NameExistsAsync(entity.Name))
            throw new ValidationException("Platform name already exists.");

        Platform? addedEntity;
        try
        {
            addedEntity = await _repository.AddAsync(entity);
        }
        catch (ArgumentNullException ex)
        {
            throw new ValidationException("PlatformCreateDto cannot be null.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ConflictException("Failed to add platform. Please check the data and try again.", ex);
        }
        catch (Exception ex)
        {
            throw new InternalErrorException(
                "An unexpected error occurred while adding the platform. Please try again later.", ex);
        }

        if (addedEntity == null)
            throw new BadRequestException("An unexpected error occurred while adding the platform.");

        return _mapper.Map<PlatformDto>(addedEntity);
    }

    public async Task UpdateGameRefsAsync(Guid platformId, List<Guid> gameIds)
    {
        List<Game> gameEntities;

        try
        {
            gameEntities = _mapper.Map<List<Game>>(gameIds);
        }
        catch (AutoMapperMappingException e)
        {
            Exception? inner = e.InnerException;

            if (inner is AggregateException agg)
                inner = agg.Flatten().InnerExceptions
                    .FirstOrDefault(exception => exception is KeyNotFoundException) ?? agg;

            if (inner is KeyNotFoundException)
                throw new NotFoundException("One or more Games were not found.");

            throw new InternalErrorException("An error occurred while mapping the GUID to the Game entity.");
        }

        if (_repository is not IPlatformRepository platformRepository)
            throw new InvalidOperationException("Repository does not support game updates.");

        try
        {
            await platformRepository.UpdateGameRefsAsync(platformId, gameEntities);
        }
        catch (ArgumentNullException e)
        {
            throw new ValidationException("The argument cannot be null.", e);
        }
        catch (KeyNotFoundException e)
        {
            throw new NotFoundException("Platform cannot be found.", e);
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new ConflictException("An error occured while updating.", e);
        }
        catch (Exception e)
        {
            throw new InternalErrorException("An unexpected error occurred while updating the entity.", e);
        }
    }

    public override async Task<bool> UpdateAsync(Guid id, PlatformCreateDto? dto)
    {
        if (dto is null)
            return false;

        var entity = TryMap(dto);
        entity.Id = id;

        if (await NameExistsAsync(entity.Name, id))
            throw new ConflictException("Platform name already exists.");

        try
        {
            return await _repository.UpdateAsync(entity);
        }
        catch (ArgumentNullException ex)
        {
            throw new ValidationException("PlatformCreateDto cannot be null.", ex);
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException("Platform not found or has been modified by another user.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ConflictException("Failed to update platform. Please check the data and try again.", ex);
        }
        catch (Exception ex)
        {
            throw new InternalErrorException(
                "An unexpected error occurred while updating the platform. Please try again later.", ex);
        }
    }

    private async Task<bool> NameExistsAsync(string name, Guid? excludeId = null)
    {
        if (String.IsNullOrEmpty(name))
            return false;

        var normalizedName = name.Trim().ToLower();
        var existing = await _repository.GetAsync(
            filter: p => p.Name.Trim().ToLower() == normalizedName && (!excludeId.HasValue || p.Id != excludeId.Value));

        return existing.Items.Any();
    }
}