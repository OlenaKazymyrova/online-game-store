﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace OnlineGameStore.BLL.Interfaces;

public interface IService<TEntity, TCreateDto, TReadDto, TUpdateDto>
    where TEntity : class
    where TCreateDto : class
    where TReadDto : class
    where TUpdateDto : class
{
    Task<TReadDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<TReadDto>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
    Task<TReadDto?> AddAsync(TCreateDto dto);
    Task<bool> UpdateAsync(TUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}