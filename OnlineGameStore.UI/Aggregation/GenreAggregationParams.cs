using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Aggregation;

public class GenreAggregationParams
{
    public string? Q { get; set; }
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Optional include parameter. Allowed values: games.
    /// </summary>
    [RegularExpression(@"^([Gg]ames)", ErrorMessage = "Invalid include value. Allowed value: games.")]
    public string? Include { get; set; }
}