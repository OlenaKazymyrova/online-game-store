using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Mapping;
using OnlineGameStore.SharedLogic.Interfaces;
using OnlineGameStore.UI.Mapping;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public abstract class ServiceMockCreator<TEntity, TDto, TService> : IMockCreator<TService>
    where TEntity : class
    where TDto : class
    where TService : class, IService<TEntity, TDto>
{
    protected readonly List<TDto> Data;
    protected readonly IMapper Mapper;
    
    protected ServiceMockCreator(List<TDto> data)
    {
        Data = data;
        Mapper = CreateMapperFromProfiles();
       
    }
    private static IMapper CreateMapperFromProfiles()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BllMappingProfile>();
            cfg.AddProfile<UiMappingProfile>();
        });
        
        configuration.AssertConfigurationIsValid();
        return configuration.CreateMapper();
    }
    
    public virtual TService Create()
    {
        var mock = new Mock<TService>();
        SetupGetById(mock);
        SetupGetAll(mock);
        SetupAdd(mock);
        SetupUpdate(mock);
        SetupDelete(mock);
        return mock.Object;
    }
    
    protected virtual void SetupGetById(Mock<TService> mock)
    {
        mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => Data.FirstOrDefault(d => 
                (Guid)d.GetType().GetProperty("Id")?.GetValue(d)! == id));
    }
    
    
    
    protected virtual void SetupGetAll(Mock<TService> mock)
    {
        mock.Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<TEntity, bool>>?>(),
                It.IsAny<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>?>(),
                It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>?>()))
            .ReturnsAsync((
                Expression<Func<TEntity, bool>>? filter,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include) =>
            {
                
                var entities = Data.Select(d => Mapper.Map<TEntity>(d)).AsQueryable();
                
                if (filter != null) entities = entities.Where(filter);
                if (orderBy != null) entities = orderBy(entities);
                // enhance or override in future for including parameter
                
                return Mapper.Map<IEnumerable<TDto>>(entities.ToList());
            });
    }
    
    protected virtual void SetupAdd(Mock<TService> mock)
    {
        mock.Setup(x => x.AddAsync(It.IsAny<TDto>()))
            .ReturnsAsync((TDto newDto) =>
            {
                
                var idProp = newDto.GetType().GetProperty("Id");
                if (idProp != null && (Guid)idProp.GetValue(newDto)! == Guid.Empty)
                {
                    idProp.SetValue(newDto, Guid.NewGuid());
                }

                Data.Add(newDto);
                return newDto;
            });
    }
    
    protected virtual void SetupUpdate(Mock<TService> mock)
    {
        mock.Setup(x => x.UpdateAsync(It.IsAny<TDto>()))
            .ReturnsAsync((TDto dto) =>
            {
                var id = (Guid)dto.GetType().GetProperty("Id")?.GetValue(dto)!;
                var index = Data.FindIndex(d => 
                    (Guid)d.GetType().GetProperty("Id")?.GetValue(d)! == id);
                
                if (index == -1) return false;
                
                Data[index] = dto;
                return true;
            });
    }
    
    protected virtual void SetupDelete(Mock<TService> mock)
    {
        mock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = Data.FindIndex(d => 
                    (Guid)d.GetType().GetProperty("Id")?.GetValue(d)! == id);
                
                if (index == -1) return false;
                
                Data.RemoveAt(index);
                return true;
            });
    }
    
    
}