using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Aggregation;

public class GenreAggregationParams
{
    /// <summary>
    /// A search query to filter genres by name or description.
    /// </summary>
    public string? Q { get; set; }

    /// <summary>
    /// Gets only genres with a specified parentID.
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Specifies related entities to include in the response. Allowed values: games.
    /// </summary>
    [RegularExpression(@"^([Gg]ames)", ErrorMessage = "Invalid include value. Allowed value: games.")]
    public string? Include { get; set; }
}