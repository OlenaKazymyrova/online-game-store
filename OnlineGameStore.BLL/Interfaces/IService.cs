using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.BLL.Interfaces;

public interface IService<TEntity, TCreateDto, TReadDto, TUpdateDto>
    where TEntity : class
    where TCreateDto : class
    where TReadDto : class
    where TUpdateDto : class
{
    Task<TReadDto?> GetByIdAsync(Guid id);
    Task<PaginatedResponse<TReadDto>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        PagingParams? pagingParams = null);
    Task<TReadDto?> AddAsync(TCreateDto dto);
    Task<bool> UpdateAsync(TUpdateDto dto);
    Task<bool> PatchAsync(Guid id, JsonPatchDocument<TUpdateDto> patchDoc);
    Task<bool> DeleteAsync(Guid id);
}