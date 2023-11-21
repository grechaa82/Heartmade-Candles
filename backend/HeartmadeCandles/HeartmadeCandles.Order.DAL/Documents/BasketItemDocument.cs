using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.DAL.Documents;

public class BasketItemDocument
{
    public required ConfiguredCandleDocument ConfiguredCandle { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public required ConfiguredCandleFilterDocument ConfiguredCandleFilter { get; set; }
}
