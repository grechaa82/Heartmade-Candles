namespace HeartmadeCandles.API.Contracts.Requests;

public class PaginationSettingsRequest
{
    public int Limit { get; init; } = 20;

    public int Index { get; init; } = 0;
}
