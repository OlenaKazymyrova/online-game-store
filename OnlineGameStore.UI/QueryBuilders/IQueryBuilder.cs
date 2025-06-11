using OnlineGameStore.UI.Aggregation;
using System.Linq.Expressions;

namespace OnlineGameStore.UI.QueryBuilders;

public interface IQueryBuilder<TEntity, AggregationParams>
    where TEntity : DAL.Entities.TEntity
{
    public Expression<Func<TEntity, bool>>? BuildFilter(AggregationParams aggregationParams);
    public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> BuildOrderBy(AggregationParams aggregationParams);
}
