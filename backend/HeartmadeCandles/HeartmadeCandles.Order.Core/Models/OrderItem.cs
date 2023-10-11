using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Core.Models;

public class OrderItem
{
    private OrderItem(CandleDetail candleDetail, int quantity, OrderItemFilter orderItemFilter)
    {
        CandleDetail = candleDetail;
        Quantity = quantity;
        OrderItemFilter = orderItemFilter;
    }

    public CandleDetail CandleDetail { get; }
    public int Quantity { get; }
    public decimal Price => CalculatePrice() * Quantity;
    public OrderItemFilter OrderItemFilter { get; }

    public static Result<OrderItem> Create(CandleDetail candleDetail, int quantity, OrderItemFilter orderItemFilter)
    {
        var result = Result.Success();

        if (quantity <= 0)
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>($"'{nameof(quantity)}' cannot be 0 or less"));
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

        var layerColorsPrice = CandleDetail.LayerColors.Sum(l => l.CalculatePriceForGrams(gramsInLayer));

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
                    $"Candle by id: {OrderItemFilter.CandleId} does not match with candle by id: {CandleDetail.Candle.Id}"));
        }

        if (OrderItemFilter.DecorId != 0)
        {
            if (CandleDetail.Decor == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<OrderItem>(
                        $"Decor by id: {OrderItemFilter.DecorId} is not found"));
            }
            else if (CandleDetail.Decor.Id != OrderItemFilter.DecorId)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<OrderItem>(
                        $"Decor by id: {OrderItemFilter.DecorId} does not match with decor by id: {CandleDetail.Decor.Id}"));
            }
        }
        else if (CandleDetail.Decor != null)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem>(
                    $"Decor by id: {OrderItemFilter.DecorId} is found, but it should not be in"));
        }

        if (CandleDetail.NumberOfLayer.Id != OrderItemFilter.NumberOfLayerId)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem[]>(
                    $"NumberOfLayer by id: {OrderItemFilter.NumberOfLayerId} does not match with numberOfLayer by id: {CandleDetail.NumberOfLayer.Id}"));
        }

        if (CandleDetail.LayerColors.Any() && CandleDetail.NumberOfLayer.Number != CandleDetail.LayerColors.Length)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem[]>(
                    $"Number of layers '{CandleDetail.NumberOfLayer.Number}' does not match the actual number '{CandleDetail.LayerColors.Length}'"));
        }

        if (!CandleDetail.LayerColors.Any())
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem[]>(
                    "LayerColors cannot be null or empty"));
        }

        if (CandleDetail.LayerColors.Length != OrderItemFilter.LayerColorIds.Length)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem>(
                    "Length of LayerColorIds is incorrect"));
        }
        else
        {
            for (var i = 0; i < OrderItemFilter.LayerColorIds.Length; i++)
                if (OrderItemFilter.LayerColorIds[i] != CandleDetail.LayerColors[i].Id)
                {
                    result = Result.Combine(
                        result,
                        Result.Failure<OrderItem>(
                            $"LayerColor by id: {OrderItemFilter.LayerColorIds[i]} does not match with layerColor by id: {CandleDetail.LayerColors[i].Id}"));
                }
        }

        if (CandleDetail.Wick.Id != OrderItemFilter.WickId)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem[]>(
                    $"Wick by id: {OrderItemFilter.WickId} does not match with wick by id: {CandleDetail.Wick.Id}"));
        }

        if (OrderItemFilter.SmellId != 0)
        {
            if (CandleDetail.Smell == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<OrderItem>(
                        $"Smell by id: {OrderItemFilter.SmellId} is not found"));
            }
            else if (CandleDetail.Smell.Id != OrderItemFilter.SmellId)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<OrderItem>(
                        $"Smell by id: {OrderItemFilter.SmellId} does not match with smell by id: {CandleDetail.Smell.Id}"));
            }
        }
        else if (CandleDetail.Smell != null)
        {
            result = Result.Combine(
                result,
                Result.Failure<OrderItem>(
                    $"Smell by id: {OrderItemFilter.SmellId} is found, but it should not be in"));
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