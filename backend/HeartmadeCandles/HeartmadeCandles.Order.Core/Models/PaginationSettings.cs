namespace HeartmadeCandles.Order.Core.Models;

public class PaginationSettings
{
    public int PageSize { get; init; } = 10;

    public int PageIndex { get; init; } = 0;
}
