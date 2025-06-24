using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineGameStore.UI.Aggregation;

namespace OnlineGameStore.UI.QueryBuilders;

public class PlatformQueryBuilder
{
    public Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>? BuildInclude(PlatformAggregationParams aggregatingParams)
    {
        if (aggregatingParams.Include is null)
        {
            return null;
        }

        return query =>
        {
            IQueryable<Platform> result = query;

            result = result.Include(platform => platform.Games);

            return (IIncludableQueryable<Platform, object>)result;
        };
    }
}
