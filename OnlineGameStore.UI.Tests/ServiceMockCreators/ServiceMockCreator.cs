using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OnlineGameStore.BLL.Exceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Mapping.Profiles;
using OnlineGameStore.SharedLogic.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public abstract class
    ServiceMockCreator<TEntity, TCreateDto, TReadDto, TUpdateDto, TDetailedDto, TService> : IMockCreator<TService>
    where TEntity : DAL.Entities.Entity
    where TCreateDto : class
    where TReadDto : class
    where TUpdateDto : class
    where TService : class, IService<TEntity, TCreateDto, TReadDto, TUpdateDto, TDetailedDto>
{
    protected readonly List<TEntity> _data;
    protected readonly IMapper _mapper;

    protected ServiceMockCreator(List<TEntity> data)
    {
        _data = data;
        _mapper = CreateMapperFromProfiles();
    }

    private static IMapper CreateMapperFromProfiles()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BllGameMappingProfile>();
            cfg.AddProfile<BllGenreMappingProfile>();
            cfg.AddProfile<BllPlatformMappingProfile>();
            cfg.AddProfile<BllUserMappingProfile>();
            cfg.AddProfile<BllRoleMappingProfile>();
        });

        configuration.AssertConfigurationIsValid();
        return configuration.CreateMapper();
    }

    public virtual TService Create()
    {
        var mock = new Mock<TService>();
        SetupGetById(mock);
        SetupGet(mock);
        SetupAdd(mock);
        SetupUpdate(mock);
        SetupPatch(mock);
        SetupDelete(mock);
        return mock.Object;
    }

    protected virtual void SetupGetById(Mock<TService> mock)
    {
        mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var entity = _data.FirstOrDefault(d =>
                    (Guid)d.GetType().GetProperty("Id")?.GetValue(d)! == id);
                if (entity == null)
                    throw new NotFoundException("Entity not found.");

                return _mapper.Map<TReadDto>(entity);
            });
    }

    protected virtual void SetupGet(Mock<TService> mock)
    {
        mock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<TEntity, bool>>?>(),
                It.IsAny<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>?>(),
                It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>?>(),
                It.IsAny<PagingParams>(),
                It.IsAny<HashSet<string>?>()))
            .ReturnsAsync((
                Expression<Func<TEntity, bool>>? filter,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
                PagingParams? pagingParams,
                HashSet<string>? explicitIncludes) =>
            {
                pagingParams ??= new PagingParams();

                var entities = _data.Select(d => _mapper.Map<TEntity>(d)).AsQueryable();

                /* NOTE: this include here is pointless as it only work with EF
                if (include != null)
                    entities = include(entities);
                */

                if (filter != null)
                    entities = entities.Where(filter);

                if (orderBy != null)
                    entities = orderBy(entities);

                var totalCount = entities.Count();

                var pagedEntities = entities
                    .Skip((pagingParams.Page - 1) * pagingParams.PageSize)
                    .Take(pagingParams.PageSize)
                    .ToList();

                var mappedItems = _mapper.Map<IEnumerable<TDetailedDto>>(
                    pagedEntities,
                    opts =>
                    {
                        opts.Items["IncludeGenres"] = explicitIncludes?.Contains("genres") ?? false;
                        opts.Items["IncludePlatforms"] = explicitIncludes?.Contains("platforms") ?? false;
                    }
                );

                return new PaginatedResponse<TDetailedDto>
                {
                    Items = mappedItems,
                    Pagination = new PaginationMetadata
                    {
                        Page = pagingParams.Page,
                        PageSize = pagingParams.PageSize,
                        TotalItems = totalCount,
                        TotalPages = (int)Math.Ceiling((double)totalCount / pagingParams.PageSize)
                    }
                };
            });
    }

    protected virtual void SetupAdd(Mock<TService> mock)
    {
        mock.Setup(x => x.AddAsync(It.IsAny<TCreateDto>()))
            .ReturnsAsync((TCreateDto createDto) =>
            {
                if (createDto == null)
                    throw new ValidationException("Create DTO cannot be null.");

                var entity = _mapper.Map<TEntity>(createDto);

                if (entity == null)
                    throw new ValidationException("Failed to map DTO to entity.");

                entity.Id = Guid.NewGuid();
                _data.Add(entity);

                return _mapper.Map<TReadDto>(entity);
            });
    }

    protected virtual void SetupUpdate(Mock<TService> mock)
    {
        mock.Setup(x => x.UpdateAsync(
                It.IsAny<Guid>(),
                It.IsAny<TCreateDto>()))
            .ReturnsAsync((Guid id, TCreateDto updateDto) =>
            {
                if (updateDto == null)
                    throw new ValidationException("Update DTO cannot be null.");

                var index = _data.FindIndex(d =>
                    (Guid)d.GetType().GetProperty("Id")?.GetValue(d)! == id);

                if (index == -1)
                    throw new NotFoundException($"Entity with ID {id} not found.");

                _mapper.Map(updateDto, _data[index]);
                _data[index].Id = id;

                return true;
            });
    }

    protected virtual void SetupPatch(Mock<TService> mock)
    {
        mock.Setup(x => x.PatchAsync(It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<TUpdateDto>>()))
            .ReturnsAsync((Guid id, JsonPatchDocument<TUpdateDto> patchDoc) =>
            {
                if (patchDoc == null)
                    throw new ValidationException("Patch document cannot be null.");

                var index = _data.FindIndex(d =>
                    (Guid)d.GetType().GetProperty("Id")?.GetValue(d)! == id);

                if (index == -1)
                    throw new NotFoundException($"Entity with ID {id} not found.");

                var entity = _data[index];
                var dto = _mapper.Map<TUpdateDto>(entity);

                patchDoc.ApplyTo(dto);
                _mapper.Map(dto, entity);

                _data[index] = entity;
                return true;
            });
    }

    protected virtual void SetupDelete(Mock<TService> mock)
    {
        mock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = _data.FindIndex(d =>
                    (Guid)d.GetType().GetProperty("Id")?.GetValue(d)! == id);

                if (index == -1)
                    throw new NotFoundException($"Entity with ID {id} not found.");

                _data.RemoveAt(index);
                return true;
            });
    }
}