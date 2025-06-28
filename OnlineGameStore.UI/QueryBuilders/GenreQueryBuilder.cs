using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.UI.Aggregation;
using System.Linq.Expressions;

namespace OnlineGameStore.UI.QueryBuilders;

public class GenreQueryBuilder : IQueryBuilder<Genre, GenreAggregationParams>
{
    public Expression<Func<Genre, bool>>? BuildFilter(GenreAggregationParams aggregationParams)
    {
        Expression<Func<Genre, bool>>? filter = null;

        if (aggregationParams.ParentId is Guid parentId)
        {
            filter = (genre => genre.ParentId == parentId);
        }

        if (!string.IsNullOrWhiteSpace(aggregationParams.Q))
        {
            var searchTerm = aggregationParams.Q.Trim().ToLower();

            Expression<Func<Genre, bool>> searchFilter = (genre =>
                genre.Name.ToLower().Contains(searchTerm) ||
                genre.Description.ToLower().Contains(searchTerm));

            if (filter == null)
            {
                filter = searchFilter;
            }
            else
            {
                var parameter = Expression.Parameter(typeof(Genre));
                var combined = Expression.AndAlso(
                    Expression.Invoke(filter, parameter),
                    Expression.Invoke(searchFilter, parameter));
                filter = Expression.Lambda<Func<Genre, bool>>(combined, parameter);
            }
        }

        return filter;
    }


    public Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>? BuildInclude(
        GenreAggregationParams aggregationParams)
    {
        if (String.IsNullOrEmpty(aggregationParams.Include))
            return null;

        return query =>
        {
            IQueryable<Genre> result = query;

            result = result.Include(genre => genre.Games);

            return (IIncludableQueryable<Genre, object>)result;
        };
    }
}