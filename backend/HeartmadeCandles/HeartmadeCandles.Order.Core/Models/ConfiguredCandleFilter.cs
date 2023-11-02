namespace HeartmadeCandles.Order.Core.Models;

public class ConfiguredCandleFilter
{
    public int CandleId { get; set; }

    public int DecorId { get; set; }

    public int NumberOfLayerId { get; set; }

    public required int[] LayerColorIds { get; set; }

    public int SmellId { get; set; }
    
    public int WickId { get; set; }

    public int Quantity { get; set; }
}