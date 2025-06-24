using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Aggregation;

public class PlatformAggregationParams
{
    [RegularExpression(@"^([Gg]ames)")] 
    public string? Include { get; set; }
}

