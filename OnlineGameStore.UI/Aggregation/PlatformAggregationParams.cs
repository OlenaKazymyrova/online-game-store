using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Aggregation;

public class PlatformAggregationParams
{
    /// <summary>
    /// Specifies related entities to include in the response. Allowed values: games.
    /// </summary>
    [RegularExpression(@"^([Gg]ames)", ErrorMessage = "Invalid include value. Allowed values: games.")]
    public string? Include { get; set; }
}