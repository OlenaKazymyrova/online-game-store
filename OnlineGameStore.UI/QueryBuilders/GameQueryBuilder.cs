using OnlineGameStore.DAL.Entities;
using OnlineGameStore.UI.Aggregation;
using System.Linq.Expressions;

namespace OnlineGameStore.UI.QueryBuilders;

public class GameQueryBuilder : IQueryBuilder<Game, GameAggregationParams>
{
    public Expression<Func<Game, bool>>? BuildFilter(GameAggregationParams aggregationParams)
    {
        bool hasNameFilter = !string.IsNullOrWhiteSpace(aggregationParams.Name);
        bool hasMinPriceFilter = aggregationParams.MinPrice.HasValue;
        bool hasMaxPriceFilter = aggregationParams.MaxPrice.HasValue;

        if (hasNameFilter || hasMinPriceFilter || hasMaxPriceFilter)
        {
            return game =>
                (!hasNameFilter || game.Name.Contains(aggregationParams.Name!)) &&
                (!hasMinPriceFilter || game.Price >= aggregationParams.MinPrice) &&
                (!hasMaxPriceFilter || game.Price <= aggregationParams.MaxPrice);
        }

        return null;
    }

    public Func<IQueryable<Game>, IOrderedQueryable<Game>> BuildOrderBy(GameAggregationParams aggregationParams)
    {
        var sortBy = aggregationParams.SortBy?.ToLower() ?? "name";
        var isDescending = aggregationParams.SortOrder?.ToLower() == "desc";

        return sortBy switch
        {
            "name" => q => isDescending ? q.OrderByDescending(g => g.Name) : q.OrderBy(g => g.Name),
            "price" => q => isDescending ? q.OrderByDescending(g => g.Price) : q.OrderBy(g => g.Price),
            "releasedate" => q => isDescending ? q.OrderByDescending(g => g.ReleaseDate) : q.OrderBy(g => g.ReleaseDate),
            _ => throw new InvalidOperationException("Unexpected sortBy value.")
        };
    }
}
