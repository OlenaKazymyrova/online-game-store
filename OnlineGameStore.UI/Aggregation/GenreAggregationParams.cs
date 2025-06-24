using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Aggregation;

public class GenreAggregationParams
{
    public Guid? ParentId { get; set; }

    [RegularExpression(@"^([Gg]ames)", ErrorMessage = "Invalid include value. Allowed value: games.")]
    public string? Include { get; set; }
}

