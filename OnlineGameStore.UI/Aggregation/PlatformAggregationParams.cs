using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Aggregation;

public class PlatformAggregationParams
{
    /// <summary>
    /// Optional include parameter. Allowed values: games.
    /// </summary>
    [RegularExpression(@"^([Gg]ames)", ErrorMessage = "Invalid include value. Allowed values: games.")]
    public string? Include { get; set; }
}