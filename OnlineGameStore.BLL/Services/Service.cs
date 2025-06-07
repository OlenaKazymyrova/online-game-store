using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.BLL.Services;

public abstract class Service<TEntity, TCreateDto, TReadDto, TUpdateDto> : IService<TEntity, TCreateDto, TReadDto, TUpdateDto>
    where TEntity : class
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

    public virtual async Task<PaginatedResponse<TReadDto>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        PagingParams? pagingParams = null)
    {
        var paginatedResponse = await _repository.GetAsync(filter, orderBy, include, pagingParams);

        return new PaginatedResponse<TReadDto>
        {
            Items = _mapper.Map<IEnumerable<TReadDto>>(paginatedResponse.Items),
            Pagination = paginatedResponse.Pagination
        };
    }

    public virtual async Task<TReadDto?> AddAsync(TCreateDto dto)
    {
        if (dto is null) return null;

        var entity = _mapper.Map<TEntity>(dto);
        var addedEntity = await _repository.AddAsync(entity);
        return addedEntity == null ? null : _mapper.Map<TReadDto>(addedEntity);
    }

    public virtual async Task<bool> UpdateAsync(TUpdateDto dto)
    {
        if (dto is null) return false;
        var entity = _mapper.Map<TEntity>(dto);
        return await _repository.UpdateAsync(entity);
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}