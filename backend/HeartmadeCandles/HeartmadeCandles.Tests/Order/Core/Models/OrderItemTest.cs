using Bogus;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.UnitTests.Order.Core.Models;

public class OrderItemTest
{
    private static readonly Faker _faker = new();

    /*[Theory]
    [MemberData(nameof(GenerateData))]
    public void Create_ValidParameters_ReturnsSuccess(
        CandleDetail candleDetail,
        int quantity,
        OrderItemFilter orderItemFilter)
    {
        // Arrange

        // Act
        var result = OrderItem.Create(candleDetail, quantity, orderItemFilter);

        // Assert
        Assert.True(result.IsSuccess);
    }

    public static IEnumerable<object[]> GenerateData()
    {
        for (var i = 0; i < 100; i++)
        {
            var candleDetail = GenerateOrderData.GenerateCandleDetail();
            var quantity = _faker.Random.Number(1, 100);

            yield return new object[]
            {
                candleDetail,
                quantity,
                GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity)
            };
        }
    }

    [Fact]
    public void Create_ZeroOrLessQuantity_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(-10000, 0);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);

        // Act
        var result = OrderItem.Create(candleDetail, quantity, orderItemFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("'quantity' cannot be 0 or less", result.Error);
    }

    [Theory]
    [MemberData(nameof(GenerateData))]
    public void CheckIsOrderItemMissing_ValidParameters_ReturnsSuccess(
        CandleDetail candleDetail,
        int quantity,
        OrderItemFilter orderItemFilter)
    {
        // Arrange
        var orderItem = OrderItem.Create(
            candleDetail,
            quantity,
            orderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void CheckIsOrderItemMissing_WithMismatchedCandle_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);

        var newOrderItemFilter = new OrderItemFilter(
            _faker.Random.Number(0, 10000),
            orderItemFilter.DecorId,
            orderItemFilter.NumberOfLayerId,
            orderItemFilter.LayerColorIds,
            orderItemFilter.SmellId,
            orderItemFilter.WickId,
            orderItemFilter.Quantity);

        var orderItem = OrderItem.Create(
            candleDetail,
            quantity,
            newOrderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);

        Assert.NotEqual(
            orderItem.Value.OrderItemFilter.CandleId,
            orderItem.Value.CandleDetail.Candle.Id);

        Assert.Equal(
            $"Candle by id: {orderItem.Value.OrderItemFilter.CandleId} does not match with candle by id: {orderItem.Value.CandleDetail.Candle.Id}",
            result.Error);
    }

    [Fact]
    public void CheckIsOrderItemMissing_WithDecorNotFoundInCandleDetail_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);

        var newCandleDetail = new CandleDetail(
            candleDetail.Candle,
            null,
            candleDetail.LayerColors,
            candleDetail.NumberOfLayer,
            candleDetail.Smell,
            candleDetail.Wick);

        var orderItem = OrderItem.Create(
            newCandleDetail,
            quantity,
            orderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal($"Decor by id: {orderItem.Value.OrderItemFilter.DecorId} is not found", result.Error);
    }

    [Fact]
    public void CheckIsOrderItemMissing_WithMismatchedDecorId_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);

        var newCandleDetail = new CandleDetail(
            candleDetail.Candle,
            GenerateOrderData.GenerateDecor(),
            candleDetail.LayerColors,
            candleDetail.NumberOfLayer,
            candleDetail.Smell,
            candleDetail.Wick);

        var orderItem = OrderItem.Create(
            newCandleDetail,
            quantity,
            orderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);

        Assert.Equal(
            $"Decor by id: {orderItem.Value.OrderItemFilter.DecorId} does not match with decor by id: {orderItem.Value.CandleDetail.Decor?.Id}",
            result.Error);
    }

    [Fact]
    public void CheckIsOrderItemMissing_WithMissingDecorIdInOrderItemFilter_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);

        var newOrderItemFilter = new OrderItemFilter(
            orderItemFilter.CandleId,
            0,
            orderItemFilter.NumberOfLayerId,
            orderItemFilter.LayerColorIds,
            orderItemFilter.SmellId,
            orderItemFilter.WickId,
            orderItemFilter.Quantity);

        var orderItem = OrderItem.Create(
            candleDetail,
            quantity,
            newOrderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);

        Assert.Equal(
            $"Decor by id: {orderItem.Value.OrderItemFilter.DecorId} is found, but it should not be in",
            result.Error);
    }

    [Fact]
    public void CheckIsOrderItemMissing_WithMismatchedNumberOfLayer_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);

        var newOrderItemFilter = new OrderItemFilter(
            orderItemFilter.CandleId,
            orderItemFilter.DecorId,
            _faker.Random.Number(0, 10000),
            orderItemFilter.LayerColorIds,
            orderItemFilter.SmellId,
            orderItemFilter.WickId,
            orderItemFilter.Quantity);

        var orderItem = OrderItem.Create(
            candleDetail,
            quantity,
            newOrderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);

        Assert.NotEqual(
            orderItem.Value.OrderItemFilter.NumberOfLayerId,
            orderItem.Value.CandleDetail.NumberOfLayer.Id);

        Assert.Equal(
            $"NumberOfLayer by id: {orderItem.Value.OrderItemFilter.NumberOfLayerId} does not match with numberOfLayer by id: {orderItem.Value.CandleDetail.NumberOfLayer.Id}",
            result.Error);
    }

    [Fact]
    public void CheckIsOrderItemMissing_DifferentArrayLengths_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);
        var newLayerColorsIds = new int[_faker.Random.Number(0, 100)];

        foreach (var _ in newLayerColorsIds)
            newLayerColorsIds.Append(_faker.Random.Number(0, 10000));

        var newOrderItemFilter = new OrderItemFilter(
            orderItemFilter.CandleId,
            orderItemFilter.DecorId,
            orderItemFilter.NumberOfLayerId,
            newLayerColorsIds,
            orderItemFilter.SmellId,
            orderItemFilter.WickId,
            orderItemFilter.Quantity);

        var orderItem = OrderItem.Create(
            candleDetail,
            quantity,
            newOrderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Length of LayerColorIds is incorrect", result.Error);
    }

    [Fact]
    public void CheckIsOrderItemMissing_DifferentLayerColors_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);
        var newLayerColorsIds = orderItemFilter.LayerColorIds.ToList();
        var newLayerColorsId = _faker.Random.Number(0, 10000);
        newLayerColorsIds[0] = newLayerColorsId;

        var newOrderItemFilter = new OrderItemFilter(
            orderItemFilter.CandleId,
            orderItemFilter.DecorId,
            orderItemFilter.NumberOfLayerId,
            newLayerColorsIds.ToArray(),
            orderItemFilter.SmellId,
            orderItemFilter.WickId,
            orderItemFilter.Quantity);

        var orderItem = OrderItem.Create(
            candleDetail,
            quantity,
            newOrderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);

        Assert.Equal(
            $"LayerColor by id: {orderItem.Value.OrderItemFilter.LayerColorIds[0]} does not match with layerColor by id: {orderItem.Value.CandleDetail.LayerColors[0].Id}",
            result.Error);
    }

    [Fact]
    public void CheckIsOrderItemMissing_WithMismatchedWick_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);

        var newOrderItemFilter = new OrderItemFilter(
            orderItemFilter.CandleId,
            orderItemFilter.DecorId,
            orderItemFilter.NumberOfLayerId,
            orderItemFilter.LayerColorIds,
            orderItemFilter.SmellId,
            _faker.Random.Number(0, 10000),
            orderItemFilter.Quantity);

        var orderItem = OrderItem.Create(
            candleDetail,
            quantity,
            newOrderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotEqual(orderItem.Value.OrderItemFilter.WickId, orderItem.Value.CandleDetail.Wick.Id);

        Assert.Equal(
            $"Wick by id: {orderItem.Value.OrderItemFilter.WickId} does not match with wick by id: {orderItem.Value.CandleDetail.Wick.Id}",
            result.Error);
    }

    [Fact]
    public void CheckIsOrderItemMissing_WithSmellNotFoundInCandleDetail_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);

        var newCandleDetail = new CandleDetail(
            candleDetail.Candle,
            candleDetail.Decor,
            candleDetail.LayerColors,
            candleDetail.NumberOfLayer,
            null,
            candleDetail.Wick);

        var orderItem = OrderItem.Create(
            newCandleDetail,
            quantity,
            orderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal($"Smell by id: {orderItem.Value.OrderItemFilter.SmellId} is not found", result.Error);
    }

    [Fact]
    public void CheckIsOrderItemMissing_WithMismatchedSmellId_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);

        var newCandleDetail = new CandleDetail(
            candleDetail.Candle,
            candleDetail.Decor,
            candleDetail.LayerColors,
            candleDetail.NumberOfLayer,
            GenerateOrderData.GenerateSmell(),
            candleDetail.Wick);

        var orderItem = OrderItem.Create(
            newCandleDetail,
            quantity,
            orderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);

        Assert.Equal(
            $"Smell by id: {orderItem.Value.OrderItemFilter.SmellId} does not match with smell by id: {orderItem.Value.CandleDetail.Smell?.Id}",
            result.Error);
    }

    [Fact]
    public void CheckIsOrderItemMissing_WithMissingSmellIdInOrderItemFilter_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateCandleDetail();
        var quantity = _faker.Random.Number(0, 10000);
        var orderItemFilter = GenerateOrderData.GenerateOrderItemFilter(candleDetail, quantity);

        var newOrderItemFilter = new OrderItemFilter(
            orderItemFilter.CandleId,
            orderItemFilter.DecorId,
            orderItemFilter.NumberOfLayerId,
            orderItemFilter.LayerColorIds,
            0,
            orderItemFilter.WickId,
            orderItemFilter.Quantity);

        var orderItem = OrderItem.Create(
            candleDetail,
            quantity,
            newOrderItemFilter);

        // Act
        var result = orderItem.Value.CheckIsOrderItemMissing();

        // Assert
        Assert.True(orderItem.IsSuccess);
        Assert.True(result.IsFailure);

        Assert.Equal(
            $"Smell by id: {orderItem.Value.OrderItemFilter.SmellId} is found, but it should not be in",
            result.Error);
    }

    // [Theory]
    // [MemberData(nameof(GenerateData))]
    public void CalculatePrice_DifferenceBetweenCandlesEqualsDifferenceBetweenCandlePrices(
        CandleDetail candleDetail,
        int quantity,
        OrderItemFilter orderItemFilter)
    {
        *//*// Arrange
        var orderItem = OrderItem.Create(candleDetail, quantity, orderItemFilter).Value;

        var newPrice = _faker.Random.Number(1, 10000) * _faker.Random.Decimal();
        var newCandle = GenerateOrderData.GenerateCandle();

        var newOrderItem = OrderItem.Create(
            new CandleDetail(
                newCandle,
                orderItem.CandleDetail.Decor,
                orderItem.CandleDetail.LayerColors,
                orderItem.CandleDetail.NumberOfLayer,
                orderItem.CandleDetail.Smell,
                orderItem.CandleDetail.Wick),
            orderItem.Quantity,
            orderItem.OrderItemFilter).Value;

        // Act
        var priceDifference = orderItem.Price - newOrderItem.Price;
        var candlePriceDifference = orderItem.CandleDetail.Candle.Price - newOrderItem.CandleDetail.Candle.Price;

        // Assert
        Assert.NotEqual(orderItem.CandleDetail.Candle.Price, newOrderItem.CandleDetail.Candle.Price);
        Assert.Equal(orderItem.CandleDetail.Decor?.Price, newOrderItem.CandleDetail.Decor?.Price);
        Assert.Equal(orderItem.CandleDetail.LayerColors[0].PricePerGram, newOrderItem.CandleDetail.LayerColors[0].PricePerGram);
        Assert.Equal(orderItem.CandleDetail.NumberOfLayer.Number, newOrderItem.CandleDetail.NumberOfLayer.Number);
        Assert.Equal(orderItem.CandleDetail.Wick.Price, newOrderItem.CandleDetail.Wick.Price);
        Assert.Equal(orderItem.CandleDetail.Smell?.Price, newOrderItem.CandleDetail.Smell?.Price);

        Assert.Equal(priceDifference, candlePriceDifference);*//*

        // Arrange
        var orderItem = OrderItem.Create(
                candleDetail,
                quantity,
                orderItemFilter)
            .Value;

        var orderItem1 = OrderItem.Create(
                new CandleDetail(
                    new Candle(
                        1,
                        "string",
                        "string",
                        orderItem.CandleDetail.Candle.Price,
                        100,
                        new Image[]
                        {
                            new("string", "string")
                        }),
                    new Decor(
                        1,
                        "string",
                        "string",
                        1,
                        new Image[]
                        {
                            new("string", "string")
                        }),
                    new LayerColor[]
                    {
                        new(
                            1,
                            "string",
                            "string",
                            1,
                            new Image[]
                            {
                                new("string", "string")
                            })
                    },
                    new NumberOfLayer(1, 1),
                    new Smell(1, "string", "string", 1),
                    new Wick(
                        1,
                        "string",
                        "string",
                        1,
                        new Image[]
                        {
                            new("string", "string")
                        })),
                1,
                orderItemFilter)
            .Value;

        var orderItem2 = OrderItem.Create(
                new CandleDetail(
                    new Candle(
                        1,
                        "string",
                        "string",
                        _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
                        100,
                        new Image[]
                        {
                            new("string", "string")
                        }),
                    new Decor(
                        1,
                        "string",
                        "string",
                        1,
                        new Image[]
                        {
                            new("string", "string")
                        }),
                    new LayerColor[]
                    {
                        new(
                            1,
                            "string",
                            "string",
                            1,
                            new Image[]
                            {
                                new("string", "string")
                            })
                    },
                    new NumberOfLayer(1, 1),
                    new Smell(1, "string", "string", 1),
                    new Wick(
                        1,
                        "string",
                        "string",
                        1,
                        new Image[]
                        {
                            new("string", "string")
                        })),
                1,
                orderItemFilter)
            .Value;

        // Act
        var priceDifference = orderItem1.Price - orderItem2.Price;
        var candlePriceDifference = orderItem1.CandleDetail.Candle.Price - orderItem2.CandleDetail.Candle.Price;

        // Assert
        Assert.Equal(priceDifference, candlePriceDifference);
    }*/
}