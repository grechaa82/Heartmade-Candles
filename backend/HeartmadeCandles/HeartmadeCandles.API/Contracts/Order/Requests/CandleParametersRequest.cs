using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class CandleParametersRequest
{
    public string? TypeFilter { get; set; }

    public PaginationSettings PaginationSettings { get; set; } = new PaginationSettings();
}