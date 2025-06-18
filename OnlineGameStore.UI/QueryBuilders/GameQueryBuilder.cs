using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
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
        var sortBy = aggregationParams.SortBy.ToLower();
        var isDescending = aggregationParams.SortOrder.ToLower() == "desc";

        return sortBy switch
        {
            "name" => q => isDescending ? q.OrderByDescending(g => g.Name) : q.OrderBy(g => g.Name),
            "price" => q => isDescending ? q.OrderByDescending(g => g.Price) : q.OrderBy(g => g.Price),
            "releasedate" => q => isDescending ? q.OrderByDescending(g => g.ReleaseDate) : q.OrderBy(g => g.ReleaseDate),
            _ => throw new InvalidOperationException("Unexpected sortBy value.")
        };
    }

    public Func<IQueryable<Game>, IIncludableQueryable<Game, object>>? BuildInclude(GameAggregationParams aggregationParams)
    {
        if (string.IsNullOrWhiteSpace(aggregationParams.Include))
        {
            return null;
        }

        var includeParams = aggregationParams.Include?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => s.ToLower())
            .ToHashSet() ?? new HashSet<string>();

        return query =>
        {
            IQueryable<Game> result = query;

            if (includeParams.Contains("genres"))
                result = result.Include(game => game.Genres);

            if (includeParams.Contains("platforms"))
                result = result.Include(game => game.Platforms);

            return (IIncludableQueryable<Game, object>)result;
        };
    }
}
