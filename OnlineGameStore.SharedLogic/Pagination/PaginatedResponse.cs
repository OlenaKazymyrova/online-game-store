
namespace OnlineGameStore.SharedLogic.Pagination;

public class PaginatedResponse<T>
{
    public required IEnumerable<T> Items { get; set; }
    public required PaginationMetadata Pagination { get; set; }
}
