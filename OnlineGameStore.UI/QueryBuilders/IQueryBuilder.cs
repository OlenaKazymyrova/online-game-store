using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.DAL.Entities;
using System.Linq.Expressions;

namespace OnlineGameStore.UI.QueryBuilders;

public interface IQueryBuilder<TEntity, in TAggregationParams>
    where TEntity : Entity
{
    public Expression<Func<TEntity, bool>>? BuildFilter(TAggregationParams aggregationParams);

    public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? BuildInclude(TAggregationParams aggregationParams);
}
