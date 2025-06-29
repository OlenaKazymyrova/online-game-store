using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Aggregation;

/// <summary>
/// Represents the query parameters used for aggregating and filtering games.
/// </summary>
public class GameAggregationParams
{
    /// <summary>
    /// A search query to filter games by name or description.
    /// </summary>
    public string? Q { get; set; }

    /// <summary>
    /// The ID of the genre to filter games by.
    /// </summary>
    public Guid? GenreId { get; set; }

    /// <summary>
    /// The ID of the platform to filter games by.
    /// </summary>
    public Guid? PlatformId { get; set; }

    /// <summary>
    /// The order in which to sort the results. Allowed values: "asc", "desc".
    /// Defaults to "asc".
    /// </summary>
    [RegularExpression("^(asc|desc)$", ErrorMessage = "Invalid sortOrder value. Allowed values: asc, desc.")]
    public string SortOrder { get; set; } = "asc";

    /// <summary>
    /// The field to sort the results by. Allowed values: "name", "price", "releaseDate".
    /// Defaults to "name".
    /// </summary>
    [RegularExpression("^(name|price|release[Dd]ate)$", ErrorMessage = "Invalid sortBy value. Allowed values: name, price, releaseDate.")]
    public string SortBy { get; set; } = "name";

    /// <summary>
    /// Filters games by partial name match.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The minimum price to filter games by. Must be non-negative.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "minPrice must be a non-negative number.")]
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// The maximum price to filter games by. Must be non-negative.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "maxPrice must be a non-negative number.")]
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Specifies related entities to include in the response. Allowed values: "genres", "platforms", or both.
    /// </summary>
    [RegularExpression(@"^(genres|platforms)(,(?!\1)(genres|platforms))?$", ErrorMessage = "Invalid include value. Allowed values: genres, platforms.")]
    public string? Include { get; set; }
}
