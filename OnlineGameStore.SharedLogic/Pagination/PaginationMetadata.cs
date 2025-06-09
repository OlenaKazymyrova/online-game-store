
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.SharedLogic.Pagination;

public class PaginationMetadata
{
    public required int Page { get; set; }
    public required int PageSize { get; set; }
    public required int TotalItems { get; set; }
    public required int TotalPages { get; set; }
}
