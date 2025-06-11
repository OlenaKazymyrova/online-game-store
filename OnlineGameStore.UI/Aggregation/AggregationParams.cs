using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.Aggregation
{
    public abstract class AggregationParams
    {
        public virtual string SortBy { get; set; } = "name";

        [RegularExpression("^(asc|desc)$", ErrorMessage = "Invalid sortOrder value. Allowed values: asc, desc.")]
        public virtual string SortOrder { get; set; } = "asc";

        public virtual string? Name { get; set; }
    }
}
