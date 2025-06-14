﻿using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Aggregation;
public class GameAggregationParams : AggregationParams
{
    [RegularExpression("^(name|price|releasedate)$", ErrorMessage = "Invalid sortBy value. Allowed values: name, price, releaseDate.")]
    public override string SortBy { get; set; } = "name";

    public override string? Name { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "minPrice must be a non-negative number.")]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "maxPrice must be a non-negative number.")]
    public decimal? MaxPrice { get; set; }
}
