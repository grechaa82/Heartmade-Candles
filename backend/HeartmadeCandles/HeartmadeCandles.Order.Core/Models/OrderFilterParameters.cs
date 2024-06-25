namespace HeartmadeCandles.Order.Core.Models;

public class OrderFilterParameters
{
    public string? SortBy { get; init; }

    public bool Ascending { get; init; } = true;

    public DateTime? CreatedFrom { get; init; }

    public DateTime? CreatedTo { get; init; }

    public OrderStatus? Status { get; init; }

    public PaginationSettings Pagination { get; init; } = new PaginationSettings();
}