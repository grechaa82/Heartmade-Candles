using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Core.Models;

public class OrderItem
{
    public OrderItem(CandleDetail candleDetail, int quantity, OrderItemFilter orderItemFilter)
    {
        CandleDetail = candleDetail;
        Quantity = quantity;
        Price = Math.Round(CalculatePrice() * quantity);
        OrderItemFilter = orderItemFilter;
    }

    public CandleDetail CandleDetail { get; }
    public int Quantity { get; }
    public decimal Price { get; private set; }
    public OrderItemFilter OrderItemFilter { get; }

    private decimal CalculatePrice()
    {
        var decorPrice = CandleDetail.Decor != null ? CandleDetail.Decor.Price : 0;
        var smellPrice = CandleDetail.Smell != null ? CandleDetail.Smell.Price : 0;

        var price = CandleDetail.Candle.Price + decorPrice + smellPrice + CandleDetail.Wick.Price;

        var gramsInLayer = CandleDetail.Candle.WeightGrams / CandleDetail.NumberOfLayer.Number;
        var layerColorsPrice = CandleDetail.LayerColors.Sum(layerColor => gramsInLayer * layerColor.PricePerGram);
        price += layerColorsPrice;

        return price;
    }

    public Result<OrderItem> CheckIsOrderItemMissing()
    {
        if (CandleDetail.Candle.Id != OrderItemFilter.CandleId
            || (CandleDetail.Decor != null
                && CandleDetail.Decor.Id != OrderItemFilter.DecorId)
            || CandleDetail.NumberOfLayer.Id != OrderItemFilter.NumberOfLayerId
            || CandleDetail.NumberOfLayer.Number != CandleDetail.LayerColors.Length
            || CandleDetail.Wick.Id != OrderItemFilter.WickId
            || (CandleDetail.Smell != null
                && CandleDetail.Smell.Id != OrderItemFilter.SmellId))
        {
            return Result.Failure<OrderItem>("");
        }

        return Result.Success(this);
    }

    public override string ToString()
    {
        var layerColors = string.Join(", ", CandleDetail.LayerColors.Select(lc => lc.Title));
        var decor = CandleDetail.Decor != null ? $"Decor={CandleDetail.Decor.Title}" : string.Empty;
        var smell = CandleDetail.Smell != null ? $"Smell={CandleDetail.Smell.Title}" : string.Empty;

        return $"Candle={CandleDetail.Candle.Title}, NumberOfLayer={CandleDetail.NumberOfLayer.Number}, " +
               $"LayerColor={layerColors}" + (decor != string.Empty ? $", {decor}" : string.Empty) +
               (smell != string.Empty ? $", {smell}" : string.Empty) +
               $", Wick={CandleDetail.Wick.Title}, Quantity={Quantity}";
    }
}