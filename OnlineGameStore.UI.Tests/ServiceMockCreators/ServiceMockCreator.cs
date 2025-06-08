using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Mapping;
using OnlineGameStore.SharedLogic.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public abstract class ServiceMockCreator<TEntity, TCreateDto, TReadDto, TUpdateDto, TService> : IMockCreator<TService>
    where TEntity : class
    where TCreateDto : class
    where TReadDto : class
    where TUpdateDto : class
    where TService : class, IService<TEntity, TCreateDto, TReadDto, TUpdateDto>
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
        var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<BllMappingProfile>(); });

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
        SetupDelete(mock);
        return mock.Object;
    }

    protected virtual void SetupGetById(Mock<TService> mock)
    {
        mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => _mapper.Map<TReadDto>(_data.FirstOrDefault(d =>
                (Guid)d.GetType().GetProperty("Id")?.GetValue(d)! == id)));
    }


    protected virtual void SetupGet(Mock<TService> mock)
    {
        mock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<TEntity, bool>>?>(),
                It.IsAny<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>?>(),
                It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>?>(),
                It.IsAny<PagingParams>()))
            .ReturnsAsync((
                Expression<Func<TEntity, bool>>? filter,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
                PagingParams? pagingParams) =>
            {
                pagingParams ??= new PagingParams();

                var entities = _data.Select(d => _mapper.Map<TEntity>(d)).AsQueryable();

                if (filter != null)
                    entities = entities.Where(filter);

                if (orderBy != null)
                    entities = orderBy(entities);

                int skip = (pagingParams.Page - 1) * pagingParams.PageSize;
                entities = entities.Skip(skip).Take(pagingParams.PageSize);

                // enhance or override in future for including parameter

                return new PaginatedResponse<TReadDto>
                {
                    Items = _mapper.Map<IEnumerable<TReadDto>>(entities.ToList()),
                    Pagination = new PaginationMetadata
                    {
                        Page = pagingParams.Page,
                        PageSize = pagingParams.PageSize,
                        TotalItems = _data.Count(),
                        TotalPages = (int)Math.Ceiling((double)_data.Count() / pagingParams.PageSize)
                    }
                };
            });
    }

    protected virtual void SetupAdd(Mock<TService> mock)
    {
        mock.Setup(x => x.AddAsync(It.IsAny<TCreateDto>()))
            .ReturnsAsync((TCreateDto createDto) =>
            {
                var idProp = createDto.GetType().GetProperty("Id");
                if (idProp != null && (Guid)idProp.GetValue(createDto)! == Guid.Empty)  // what if there is not proprty named Id
                {
                    idProp.SetValue(createDto, Guid.NewGuid());
                }

                var entity = _mapper.Map<TEntity>(createDto);
                _data.Add(entity);

                return _mapper.Map<TReadDto>(entity);
            });
    }

    protected virtual void SetupUpdate(Mock<TService> mock)
    {
        mock.Setup(x => x.UpdateAsync(It.IsAny<TUpdateDto>()))
            .ReturnsAsync((TUpdateDto updateDto) =>
            {
                var id = (Guid)updateDto.GetType().GetProperty("Id")?.GetValue(updateDto)!;
                var index = _data.FindIndex(d =>
                    (Guid)d.GetType().GetProperty("Id")?.GetValue(d)! == id);

                if (index == -1)
                    return false;

                _mapper.Map(updateDto, _data[index]);
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

                if (index == -1) return false;

                _data.RemoveAt(index);
                return true;
            });
    }
}