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
        if (aggregationParams.ParentId is null)
            return null;

        return genre => genre.ParentId == aggregationParams.ParentId;
    }


    public Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>? BuildInclude(GenreAggregationParams aggregationParams)
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

