using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.BLL.Services;

public abstract class
    Service<TEntity, TCreateDto, TReadDto, TUpdateDto, TDetailedDto> : IService<TEntity, TCreateDto, TReadDto,
    TUpdateDto, TDetailedDto>
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
        return entity == null ? null : _mapper.Map<TReadDto>(entity);
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

    public virtual async Task<TReadDto?> AddAsync(TCreateDto dto)
    {
        if (dto is null)
            return null;

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
                return null;

            throw;
        }

        var addedEntity = await _repository.AddAsync(entity);

        return addedEntity == null ? null : _mapper.Map<TReadDto>(addedEntity);
    }

    public virtual async Task<bool> UpdateAsync(Guid id, TCreateDto dto)
    {
        if (dto is null)
            return false;

        var entity = _mapper.Map<TEntity>(dto);
        entity.Id = id;

        return await _repository.UpdateAsync(entity);
    }

    public virtual async Task<bool> PatchAsync(Guid id, JsonPatchDocument<TUpdateDto> patchDoc)
    {
        if (patchDoc == null)
            return false;

        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return false;

        var dto = _mapper.Map<TUpdateDto>(entity);

        patchDoc.ApplyTo(dto);

        _mapper.Map(dto, entity);

        return await _repository.UpdateAsync(entity);
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}