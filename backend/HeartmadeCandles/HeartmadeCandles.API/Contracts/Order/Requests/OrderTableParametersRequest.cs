using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class OrderTableParametersRequest
{
    public string? SortBy { get; init; }

    public bool Ascending { get; init; } = true;

    public DateTime? CreatedFrom { get; init; }

    public DateTime? CreatedTo { get; init; }

    public OrderStatus? Status { get; init; }

    public int pageSige { get; init; } = 10;

    public int pageIndex { get; init; } = 0;
}
