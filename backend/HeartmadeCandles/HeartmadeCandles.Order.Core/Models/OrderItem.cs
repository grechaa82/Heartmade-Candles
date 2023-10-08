using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Core.Models;

public class OrderItem
{
    private OrderItem(CandleDetail candleDetail, int quantity, OrderItemFilter orderItemFilter)
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

    public static Result<OrderItem> Create(CandleDetail candleDetail, int quantity, OrderItemFilter orderItemFilter)
    {
        var result = Result.Success();

        if (candleDetail == null)
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>($"'{nameof(candleDetail)}' cannot be null"));
        }

        if (quantity <= 0)
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>($"'{nameof(quantity)}' cannot be 0 or less"));
        }

        if (orderItemFilter == null)
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>($"'{nameof(orderItemFilter)}' cannot be null"));
        }

        if (result.IsFailure)
        {
            return Result.Failure<OrderItem>(result.Error);
        }

        var orderItem = new OrderItem(candleDetail, quantity, orderItemFilter);

        return Result.Success(orderItem);
    }

    private decimal CalculatePrice()
    {
        var decorPrice = CandleDetail.Decor?.Price ?? 0;
        var smellPrice = CandleDetail.Smell?.Price ?? 0;

        var price = CandleDetail.Candle.Price + decorPrice + smellPrice + CandleDetail.Wick.Price;

        var gramsInLayer = CandleDetail.Candle.WeightGrams / CandleDetail.NumberOfLayer.Number;
        var layerColorsPrice = CandleDetail.LayerColors.Sum(layerColor => gramsInLayer * layerColor.PricePerGram);
        price += layerColorsPrice;

        return price;
    }

    public Result<OrderItem> CheckIsOrderItemMissing()
    {
        var result = Result.Success();

        if (CandleDetail.Candle.Id != OrderItemFilter.CandleId)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem[]>(
                    $"Candle by id: {CandleDetail.Candle.Id} does not match with candle by id: {OrderItemFilter.CandleId}"));
        }


        if (OrderItemFilter.DecorId != 0)
        {
            if (CandleDetail.Decor != null || CandleDetail.Decor?.Id != OrderItemFilter.DecorId)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<OrderItem[]>(
                        $"Decor by id: {CandleDetail.Decor?.Id} does not match with decor by id: {OrderItemFilter.DecorId}"));
            }
        }

        if (CandleDetail.NumberOfLayer.Id != OrderItemFilter.NumberOfLayerId)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem[]>(
                    $"NumberOfLayer by id: {CandleDetail.NumberOfLayer.Id} does not match with numberOfLayer by id: {OrderItemFilter.NumberOfLayerId}"));
        }

        if (CandleDetail.NumberOfLayer.Number != CandleDetail.LayerColors.Length)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem[]>(
                    $"The number of layers '{CandleDetail.NumberOfLayer.Number}' does not match the actual number '{CandleDetail.LayerColors.Length}'"));
        }

        if (CandleDetail.Wick.Id != OrderItemFilter.WickId)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem[]>(
                    $"Wick by id: {CandleDetail.Wick.Id} does not match with wick by id: {OrderItemFilter.WickId}"));
        }

        if (CandleDetail.Smell != null && CandleDetail.Smell.Id != OrderItemFilter.SmellId)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem[]>(
                    $"Smell by id: {CandleDetail.Smell.Id} does not match with smell by id: {OrderItemFilter.SmellId}"));
        }

        if (result.IsFailure)
        {
            return Result.Failure<OrderItem>(result.Error);
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