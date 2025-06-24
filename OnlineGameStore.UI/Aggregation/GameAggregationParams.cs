using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Aggregation;

public class GameAggregationParams
{
    public string? Q { get; set; }

    public Guid? GenreId { get; set; }

    public Guid? PlatformId { get; set; }

    [RegularExpression("^(asc|desc)$", ErrorMessage = "Invalid sortOrder value. Allowed values: asc, desc.")]
    public string SortOrder { get; set; } = "asc";

    [RegularExpression("^(name|price|release[Dd]ate)$", ErrorMessage = "Invalid sortBy value. Allowed values: name, price, releaseDate.")]
    public string SortBy { get; set; } = "name";
    public string? Name { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "minPrice must be a non-negative number.")]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "maxPrice must be a non-negative number.")]
    public decimal? MaxPrice { get; set; }

    [RegularExpression(@"^(genres|platforms)(,(?!\1)(genres|platforms))?$", ErrorMessage = "Invalid include value. Allowed values: genres, platforms.")]
    public string? Include { get; set; }
}