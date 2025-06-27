using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.BLL.Exceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.BLL.Services;

public abstract class
    Service<TEntity, TCreateDto, TReadDto, TUpdateDto, TDetailedDto> :
    IService<TEntity, TCreateDto, TReadDto, TUpdateDto, TDetailedDto>
    where TEntity : DAL.Entities.TEntity
    where TCreateDto : class
    where TReadDto : class
    where TUpdateDto : class
{
    protected readonly IRepository<TEntity> _repository;
    protected readonly IMapper _mapper;

    protected Service(IRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<TReadDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException("Entity not found.");

        return _mapper.Map<TReadDto>(entity);
    }

    public virtual async Task<PaginatedResponse<TDetailedDto>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        PagingParams? pagingParams = null,
        HashSet<string>? explicitIncludes = null)
    {
        var paginatedResponse = await _repository.GetAsync(filter, orderBy, include, pagingParams);

        return new PaginatedResponse<TDetailedDto>
        {
            Items = _mapper.Map<IEnumerable<TDetailedDto>>(paginatedResponse.Items),
            Pagination = paginatedResponse.Pagination
        };
    }

    public virtual async Task<TReadDto> AddAsync(TCreateDto dto)
    {
        if (dto is null)
            throw new ValidationException("DTO is required for create.");

        TEntity entity;

        try
        {
            entity = _mapper.Map<TEntity>(dto);
        }
        catch (AutoMapperMappingException e)
        {
            Exception? inner = e.InnerException;

            if (inner is AggregateException agg)
                inner = agg.Flatten().InnerExceptions
                    .FirstOrDefault(exception => exception is KeyNotFoundException) ?? agg;

            if (inner is KeyNotFoundException)
                throw new NotFoundException("One or more properties in the DTO were not found in the entity.", inner);

            throw new InternalErrorException("An error occurred while mapping the DTO to the entity.", inner);
        }

        TEntity? addedEntity;
        try
        {
            addedEntity = await _repository.AddAsync(entity);
        }
        catch (ArgumentNullException ex)
        {
            throw new ValidationException("Entity cannot be null.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ConflictException("An error occurred while adding the entity. It may already exist.", ex);
        }
        catch (Exception ex)
        {
            throw new InternalErrorException("An unexpected error occurred while adding the entity.", ex);
        }

        if (addedEntity == null)
            throw new BadRequestException("An unexpected error occurred while adding the entity.");

        return _mapper.Map<TReadDto>(addedEntity);
    }

    public virtual async Task<bool> UpdateAsync(Guid id, TCreateDto dto)
    {
        if (dto is null)
            throw new ValidationException("DTO is required for update.");

        var entity = _mapper.Map<TEntity>(dto);
        entity.Id = id;

        try
        {
            return await _repository.UpdateAsync(entity);
        }
        catch (ArgumentNullException ex)
        {
            throw new ValidationException("Entity cannot be null.", ex);
        }
        catch (NullReferenceException ex)
        {
            throw new NotFoundException("Entity not found.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ConflictException("An error occurred while updating the entity.", ex);
        }
        catch (Exception ex)
        {
            throw new InternalErrorException("An unexpected error occurred while updating the entity.", ex);
        }
    }

    public virtual async Task<bool> PatchAsync(Guid id, JsonPatchDocument<TUpdateDto> patchDoc)
    {
        if (patchDoc == null)
            throw new ValidationException("Patch document is required.");

        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            throw new NotFoundException("Entity not found.");

        var dto = _mapper.Map<TUpdateDto>(entity);

        patchDoc.ApplyTo(dto);

        _mapper.Map(dto, entity);

        try
        {
            return await _repository.UpdateAsync(entity);
        }
        catch (ArgumentNullException ex)
        {
            throw new ValidationException("Entity cannot be null.", ex);
        }
        catch (NullReferenceException ex)
        {
            throw new NotFoundException("Entity not found.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ConflictException("An error occurred while updating the entity.", ex);
        }
        catch (Exception ex)
        {
            throw new InternalErrorException("An unexpected error occurred while updating the entity.", ex);
        }
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            return await _repository.DeleteAsync(id);
        }
        catch (ArgumentNullException ex)
        {
            throw new ValidationException("ID is required for deletion.", ex);
        }
        catch (NullReferenceException ex)
        {
            throw new NotFoundException("Entity not found.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ConflictException("An error occurred while deleting the entity.", ex);
        }
        catch (Exception ex)
        {
            throw new InternalErrorException("An unexpected error occurred while deleting the entity.", ex);
        }
    }
}