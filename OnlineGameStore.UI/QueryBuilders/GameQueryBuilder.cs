using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.UI.Aggregation;
using System.Linq.Expressions;

namespace OnlineGameStore.UI.QueryBuilders;

public class GameQueryBuilder : IQueryBuilder<Game, GameAggregationParams>
{
    public HashSet<string> RequiredIncludes { get; } = new();

    public Expression<Func<Game, bool>>? BuildFilter(GameAggregationParams aggregationParams)
    {
        var filters = new List<Expression<Func<Game, bool>>>();

        if (!string.IsNullOrWhiteSpace(aggregationParams.Q))
        {
            string searchTerm = aggregationParams.Q.Trim().ToLower();
            filters.Add(game =>
                game.Name.ToLower().Contains(searchTerm) || game.Description.ToLower().Contains(searchTerm));
        }

        if (aggregationParams.GenreId.HasValue)
        {
            RequiredIncludes.Add("genres");
            Guid genreId = aggregationParams.GenreId.Value;
            filters.Add(game => game.Genres.Any(genre => genre.Id == genreId));
        }

        if (aggregationParams.PlatformId.HasValue)
        {
            RequiredIncludes.Add("platforms");
            Guid platformId = aggregationParams.PlatformId.Value;
            filters.Add(game => game.Platforms.Any(platform => platform.Id == platformId));
        }

        if (!string.IsNullOrWhiteSpace(aggregationParams.Name))
        {
            filters.Add(game => game.Name.Contains(aggregationParams.Name));
        }

        if (aggregationParams.MinPrice.HasValue)
        {
            filters.Add(game => game.Price >= aggregationParams.MinPrice);
        }

        if (aggregationParams.MaxPrice.HasValue)
        {
            filters.Add(game => game.Price <= aggregationParams.MaxPrice);
        }

        return filters.Count > 0 ? CombineFilters(filters) : null;
    }

    public Func<IQueryable<Game>, IOrderedQueryable<Game>> BuildOrderBy(GameAggregationParams aggregationParams)
    {
        var sortBy = aggregationParams.SortBy.ToLower();
        var isDescending = aggregationParams.SortOrder.ToLower() == "desc";

        return sortBy switch
        {
            "name" => q => isDescending ? q.OrderByDescending(g => g.Name) : q.OrderBy(g => g.Name),
            "price" => q => isDescending ? q.OrderByDescending(g => g.Price) : q.OrderBy(g => g.Price),
            "releasedate" => q =>
                isDescending ? q.OrderByDescending(g => g.ReleaseDate) : q.OrderBy(g => g.ReleaseDate),
            _ => throw new InvalidOperationException("Unexpected sortBy value.")
        };
    }

    public Func<IQueryable<Game>, IIncludableQueryable<Game, object>>? BuildInclude(
        GameAggregationParams aggregationParams)
    {
        if (string.IsNullOrWhiteSpace(aggregationParams.Include))
        {
            return null;
        }

        var includeParams = aggregationParams.Include?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => s.ToLower())
            .ToHashSet() ?? new HashSet<string>();

        var allIncludes = new HashSet<string>(includeParams);
        allIncludes.UnionWith(RequiredIncludes);

        if (allIncludes.Count == 0) return null;

        return query =>
        {
            IQueryable<Game> result = query;

            if (allIncludes.Contains("genres"))
                result = result.Include(game => game.Genres);

            if (allIncludes.Contains("platforms"))
                result = result.Include(game => game.Platforms);

            return (IIncludableQueryable<Game, object>)result;
        };
    }

    private static Expression<Func<Game, bool>> CombineFilters(List<Expression<Func<Game, bool>>> filters)
    {
        var parameter = Expression.Parameter(typeof(Game));
        Expression combined = Expression.Invoke(filters[0], parameter);

        for (int i = 1; i < filters.Count; i++)
        {
            combined = Expression.AndAlso(combined, Expression.Invoke(filters[i], parameter));
        }

        return Expression.Lambda<Func<Game, bool>>(combined, parameter);
    }
}