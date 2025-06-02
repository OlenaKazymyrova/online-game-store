using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.BLL.Tests.RepositoryMockCreator;

public abstract class RepositoryMockCreator<TEntity, TRepository> : IMockCreator<TRepository>
    where TEntity : class
    where TRepository : class, IRepository<TEntity>
{
    protected readonly List<TEntity> Data;
    
    protected RepositoryMockCreator(List<TEntity> data)
    {
        Data = data;
    }
    
    public virtual TRepository Create()
    {
        var mock = new Mock<TRepository>();
        SetupGetById(mock);
        SetupGetAll(mock);
        SetupAdd(mock);
        SetupUpdate(mock);
        SetupDelete(mock);
        return mock.Object;
    }
    
    protected virtual void SetupGetById(Mock<TRepository> mock)
    {
        mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => 
            {
                var property = typeof(TEntity).GetProperty("Id");
                if (property == null) return null;
                return Data.FirstOrDefault(x => (Guid)property.GetValue(x)! == id);
            });
    }
    
    protected virtual void SetupGetAll(Mock<TRepository> mock)
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
                IQueryable<TEntity> query = Data.AsQueryable();
            
                if (include != null) query = include(query);
                if (filter != null) query = query.Where(filter);
                if (orderBy != null) query = orderBy(query);
            
                return query.ToList();
            });
    }
    
    protected virtual void SetupAdd(Mock<TRepository> mock)
    {
        mock.Setup(x => x.AddAsync(It.IsAny<TEntity>()))
            .ReturnsAsync((TEntity entity) =>
            {
                Data.Add(entity);
                return entity;
            });
    }
    
    protected virtual void SetupUpdate(Mock<TRepository> mock)
    {
        mock.Setup(x => x.UpdateAsync(It.IsAny<TEntity>()))
            .ReturnsAsync((TEntity entity) =>
            {
                var property = typeof(TEntity).GetProperty("Id");
                if (property == null) return false;
                
                var id = (Guid)property.GetValue(entity)!;
                
                var index = Data.FindIndex(x => 
                    (Guid)property.GetValue(x)! == id);
                
                if (index == -1) return false;
                
                Data[index] = entity;
                return true;
            });
    }
    
    protected virtual void SetupDelete(Mock<TRepository> mock)
    {
        mock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var property = typeof(TEntity).GetProperty("Id");
                if (property == null) return false;
                
                var index = Data.FindIndex(x => 
                    (Guid)property.GetValue(x)! == id);
                
                if (index == -1) return false;
                
                Data.RemoveAt(index);
                return true;
            });
    }
}