using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Filtering;
public class GameAggregationParams
{
    private static readonly string[] AllowedSortBy = { "name", "price", "releasedate" };
    private static readonly string[] AllowedSortOrder = { "asc", "desc" };

    [RegularExpression("^(name|price|releasedate)$", ErrorMessage = "Invalid sortBy value. Allowed values: name, price, releaseDate.")]
    public string SortBy { get; set; } = "name";

    [RegularExpression("^(asc|desc)$", ErrorMessage = "Invalid sortOrder value. Allowed values: asc, desc.")]
    public string SortOrder { get; set; } = "asc";

    public string? Name { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "minPrice must be a non-negative number.")]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "maxPrice must be a non-negative number.")]
    public decimal? MaxPrice { get; set; }
}
