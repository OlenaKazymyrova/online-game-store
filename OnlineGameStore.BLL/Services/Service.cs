using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public abstract class Service<TEntity, TDto> : IService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IMapper Mapper;

    protected Service(IRepository<TEntity> repository, IMapper mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }

    public virtual async Task<TDto?> GetByIdAsync(Guid id)
    {
        var entity = await Repository.GetByIdAsync(id);
        return entity == null ? null : Mapper.Map<TDto>(entity);
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        var entities = await Repository.GetAllAsync(filter, orderBy, include);
        return Mapper.Map<IEnumerable<TDto>>(entities);
    }

    public virtual async Task<TDto?> AddAsync(TDto dto)
    {
        if (dto is null) return null;

        var entity = Mapper.Map<TEntity>(dto);
        var addedEntity = await Repository.AddAsync(entity);
        return addedEntity == null ? null : Mapper.Map<TDto>(addedEntity);
    }

    public virtual async Task<bool> UpdateAsync(TDto dto)
    {
        if (dto is null) return false;
        var entity = Mapper.Map<TEntity>(dto);
        return await Repository.UpdateAsync(entity);
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        return await Repository.DeleteAsync(id);
    }


}