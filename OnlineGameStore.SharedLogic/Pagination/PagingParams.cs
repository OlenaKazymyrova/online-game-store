using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.SharedLogic.Pagination;

public class PagingParams
{
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
    public int Page { get; set; } = 1;
    [Range(1, MaxPageSize, ErrorMessage = "PageSize must be between 1 and 100.")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    private const int MaxPageSize = 100;
    private int _pageSize = 10;

}
