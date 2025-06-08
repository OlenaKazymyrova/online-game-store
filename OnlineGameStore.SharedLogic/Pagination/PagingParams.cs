using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.SharedLogic.Pagination;

public class PagingParams
{
    [Required]
    public int Page { get; set; } = 1;

    [Required]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    private const int MaxPageSize = 100;
    private int _pageSize = 10;

}
