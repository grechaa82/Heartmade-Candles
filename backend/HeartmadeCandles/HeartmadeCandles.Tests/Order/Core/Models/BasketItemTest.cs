using Bogus;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.UnitTests.Order.Core.Models;

public class BasketItemTest
{
    private static readonly Faker _faker = new();

    [Theory]
    [MemberData(nameof(GenerateTestDataForCreateValidParameters))]
    public void Create_ValidParameters_ReturnsSuccess(
        ConfiguredCandle candleDetail,
        decimal price,
        ConfiguredCandleFilter configuredCandleFilter)
    {
        // Arrange

        // Act
        var result = BasketItem.Create(
            configuredCandle: candleDetail,
            price: price,
            configuredCandleFilter: configuredCandleFilter);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_WithoutDecor_ReturnsSuccess()
    {
        // Arrange
        var basketItem = GenerateOrderData.GenerateBasketItem();

        var newConfiguredCandleFilter = new ConfiguredCandleFilter
        {
            CandleId  = basketItem.ConfiguredCandleFilter.CandleId,
            DecorId = 0,
            NumberOfLayerId  = basketItem.ConfiguredCandleFilter.NumberOfLayerId,
            LayerColorIds  = basketItem.ConfiguredCandleFilter.LayerColorIds,
            SmellId  = basketItem.ConfiguredCandleFilter.SmellId,
            WickId  = basketItem.ConfiguredCandleFilter.WickId,
            Quantity  = basketItem.ConfiguredCandleFilter.Quantity,
            FilterString  = basketItem.ConfiguredCandleFilter.FilterString
        };

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = basketItem.ConfiguredCandle.Candle,
            Decor = null,
            LayerColors = basketItem.ConfiguredCandle.LayerColors,
            NumberOfLayer = basketItem.ConfiguredCandle.NumberOfLayer,
            Smell = basketItem.ConfiguredCandle.Smell,
            Wick = basketItem.ConfiguredCandle.Wick
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: newConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: newConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsSuccess);
    }

    public static IEnumerable<object[]> GenerateTestDataForCreateValidParameters()
    {
        for (var i = 0; i < 100; i++)
        {
            var candleDetail = GenerateOrderData.GenerateConfiguredCandle();
            var quantity = _faker.Random.Number(1, 100);
            var price = _faker.Random.Number(1, 10000) * _faker.Random.Decimal();

            yield return new object[]
            {
                candleDetail,
                price,
                GenerateOrderData.GenerateConfiguredCandleFilter(candleDetail, quantity)
            };
        }
    }

    [Fact]
    public void Create_ZeroOrLessPrice_ReturnFailure()
    {
        // Arrange
        var candleDetail = GenerateOrderData.GenerateConfiguredCandle();
        var quantity = _faker.Random.Number(1, 100);
        var orderItemFilter = GenerateOrderData.GenerateConfiguredCandleFilter(candleDetail, quantity);
        var price = _faker.Random.Number(-10000, 0) * _faker.Random.Decimal();

        // Act
        var result = BasketItem.Create(
            configuredCandle: candleDetail,
            price: price,
            configuredCandleFilter: orderItemFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal($"{nameof(result.Value.Price)} cannot be 0 or less", result.Error);
    }

    [Fact]
    public void Create_IdCandleNotMatch_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandleFilter = new ConfiguredCandleFilter
        {
            CandleId = GenerateData.GenerateInvalidId(),
            DecorId = configuredCandleFilter.DecorId,
            NumberOfLayerId = configuredCandleFilter.NumberOfLayerId,
            LayerColorIds = configuredCandleFilter.LayerColorIds,
            SmellId = configuredCandleFilter.SmellId,
            WickId = configuredCandleFilter.WickId,
            Quantity = configuredCandleFilter.Quantity,
            FilterString = configuredCandleFilter.FilterString
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: basketItem.ConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: newConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotEqual(newConfiguredCandleFilter.CandleId, basketItem.ConfiguredCandle.Candle.Id);
        Assert.Equal(
            $"{nameof(basketItem.ConfiguredCandle.Candle)} by id: {newConfiguredCandleFilter.CandleId} " +
            $"does not match with {nameof(basketItem.ConfiguredCandle.Candle)} by id: {basketItem.ConfiguredCandle.Candle.Id}",
            result.Error);
    }

    [Fact]
    public void Create_DecorIdNotZeroAndDecorIsNull_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = basketItem.ConfiguredCandle.Candle,
            Decor = null,
            LayerColors = basketItem.ConfiguredCandle.LayerColors,
            NumberOfLayer = basketItem.ConfiguredCandle.NumberOfLayer,
            Smell = basketItem.ConfiguredCandle.Smell,
            Wick = basketItem.ConfiguredCandle.Wick
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: newConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: basketItem.ConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.True(basketItem.ConfiguredCandleFilter.DecorId != 0);
        Assert.True(newConfiguredCandle.Decor == null);
        Assert.Equal(
            $"{nameof(basketItem.ConfiguredCandle.Decor)} by id: {basketItem.ConfiguredCandleFilter.DecorId} is not found",
            result.Error);
    }

    [Fact]
    public void Create_DecorIdNotZeroAndDecorIdNotMatching_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = basketItem.ConfiguredCandle.Candle,
            Decor = GenerateOrderData.GenerateDecor(),
            LayerColors = basketItem.ConfiguredCandle.LayerColors,
            NumberOfLayer = basketItem.ConfiguredCandle.NumberOfLayer,
            Smell = basketItem.ConfiguredCandle.Smell,
            Wick = basketItem.ConfiguredCandle.Wick
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: newConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: basketItem.ConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.True(basketItem.ConfiguredCandleFilter.DecorId != 0);
        Assert.NotNull(newConfiguredCandle.Decor);
        Assert.Equal(
            $"{nameof(basketItem.ConfiguredCandle.Decor)} by id: {basketItem.ConfiguredCandleFilter.DecorId} " +
            $"does not match with {nameof(basketItem.ConfiguredCandle.Decor)} by id: {newConfiguredCandle.Decor?.Id}",
            result.Error);
    }

    [Fact]
    public void Create_DecorIdIsZeroAndDecorIsNotNull_ReturnFailure()
    {
        // Arrange
        var configuredCandle = GenerateOrderData.GenerateConfiguredCandle();

        var newConfiguredCandleFilter = new ConfiguredCandleFilter
        {
            CandleId  = configuredCandle.Candle.Id,
            DecorId  = 0,
            NumberOfLayerId  = configuredCandle.NumberOfLayer.Id,
            LayerColorIds  = configuredCandle.LayerColors.Select(l => l.Id).ToArray(),
            SmellId  = configuredCandle.Smell?.Id,
            WickId  = configuredCandle.Wick.Id,
            Quantity  = _faker.Random.Number(1, 100),
            FilterString  = _faker.Random.String()
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: configuredCandle,
            price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            configuredCandleFilter: newConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(configuredCandle.Decor);
        Assert.NotEqual(configuredCandle.Decor.Id, newConfiguredCandleFilter.DecorId);
        Assert.Equal(
            $"{nameof(configuredCandle.Decor)} by id: {configuredCandle.Decor.Id} is found, but it should not be in",
            result.Error);
    }

    [Fact]
    public void Create_DecorIdAndDecorIdAreNull_ReturnsSuccess()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = basketItem.ConfiguredCandle.Candle,
            Decor = null,
            LayerColors = basketItem.ConfiguredCandle.LayerColors,
            NumberOfLayer = basketItem.ConfiguredCandle.NumberOfLayer,
            Smell = basketItem.ConfiguredCandle.Smell,
            Wick = basketItem.ConfiguredCandle.Wick
        };

        var newConfiguredCandleFilter = new ConfiguredCandleFilter
        {
            CandleId  = basketItem.ConfiguredCandle.Candle.Id,
            DecorId  = 0,
            NumberOfLayerId  = basketItem.ConfiguredCandle.NumberOfLayer.Id,
            LayerColorIds  = basketItem.ConfiguredCandle.LayerColors.Select(l => l.Id).ToArray(),
            SmellId  = basketItem.ConfiguredCandle.Smell?.Id,
            WickId  = basketItem.ConfiguredCandle.Wick.Id,
            Quantity  = _faker.Random.Number(1, 100),
            FilterString  = _faker.Random.String()
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: newConfiguredCandle,
            price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            configuredCandleFilter: newConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(newConfiguredCandle.Decor);
        Assert.Equal(newConfiguredCandleFilter.DecorId, 0);
    }

    [Fact]
    public void Create_IdNumberOfLayerNotMatch_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandleFilter = new ConfiguredCandleFilter
        {
            CandleId = configuredCandleFilter.CandleId,
            DecorId = configuredCandleFilter.DecorId,
            NumberOfLayerId = GenerateData.GenerateInvalidId(),
            LayerColorIds = configuredCandleFilter.LayerColorIds,
            SmellId = configuredCandleFilter.SmellId,
            WickId = configuredCandleFilter.WickId,
            Quantity = configuredCandleFilter.Quantity,
            FilterString = configuredCandleFilter.FilterString
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: basketItem.ConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: newConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotEqual(newConfiguredCandleFilter.NumberOfLayerId, basketItem.ConfiguredCandle.NumberOfLayer.Id);
        Assert.Equal(
            $"{nameof(basketItem.ConfiguredCandle.NumberOfLayer)} by id: {newConfiguredCandleFilter.NumberOfLayerId} " +
            $"does not match with {nameof(basketItem.ConfiguredCandle.NumberOfLayer)} by id: {basketItem.ConfiguredCandle.NumberOfLayer.Id}",
            result.Error);
    }

    [Fact]
    public void Create_NumberNotMatchedWithLayerColorLenght_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = basketItem.ConfiguredCandle.Candle,
            Decor = basketItem.ConfiguredCandle.Decor,
            LayerColors = basketItem.ConfiguredCandle.LayerColors,
            NumberOfLayer = GenerateOrderData.GenerateNumberOfLayer(
                number: _faker.Random.Number(0, 10000),
                id: basketItem.ConfiguredCandle.NumberOfLayer.Id),
            Smell = basketItem.ConfiguredCandle.Smell,
            Wick = basketItem.ConfiguredCandle.Wick
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: newConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: configuredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotEqual(newConfiguredCandle.NumberOfLayer.Number, basketItem.ConfiguredCandle.NumberOfLayer.Number);
        Assert.Contains(
            $"The configured layer colors and their count do not match the specified criteria",
            result.Error);
    }

    [Fact]
    public void Create_LayerColorIsEmpty_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = basketItem.ConfiguredCandle.Candle,
            Decor = basketItem.ConfiguredCandle.Decor,
            LayerColors = Array.Empty<LayerColor>(),
            NumberOfLayer = basketItem.ConfiguredCandle.NumberOfLayer,
            Smell = basketItem.ConfiguredCandle.Smell,
            Wick = basketItem.ConfiguredCandle.Wick
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: newConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: configuredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Empty(newConfiguredCandle.LayerColors);
        Assert.Contains($"{nameof(newConfiguredCandle.LayerColors)} cannot be null or empty", result.Error);
    }

    [Fact]
    public void Create_LayerColorLengthNotMatch_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newLayerColors = new List<LayerColor>();
        for (var i = 0; i < _faker.Random.Number(1, 100); i++)
            newLayerColors.Add(GenerateOrderData.GenerateLayerColor(configuredCandleFilter.LayerColorIds[i]));

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = basketItem.ConfiguredCandle.Candle,
            Decor = basketItem.ConfiguredCandle.Decor,
            LayerColors = newLayerColors.ToArray(),
            NumberOfLayer = basketItem.ConfiguredCandle.NumberOfLayer,
            Smell = basketItem.ConfiguredCandle.Smell,
            Wick = basketItem.ConfiguredCandle.Wick
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: newConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: configuredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotEmpty(newConfiguredCandle.LayerColors);
        Assert.NotEqual(basketItem.ConfiguredCandle.LayerColors.Length, newConfiguredCandle.LayerColors.Length);
        Assert.Contains($"The configured layer colors and their count do not match the specified criteria", result.Error);
    }

    [Fact]
    public void Create_LayerColorIdNotMatch_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        basketItem.ConfiguredCandle.LayerColors[0] = GenerateOrderData.GenerateLayerColor();

        // Act
        var result = BasketItem.Create(
            configuredCandle: basketItem.ConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: configuredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotEqual(basketItem.ConfiguredCandleFilter.LayerColorIds[0], basketItem.ConfiguredCandle.LayerColors[0].Id);
        Assert.Equal($"{nameof(basketItem.ConfiguredCandle.LayerColors)} by id: {configuredCandleFilter.LayerColorIds[0]} " +
            $"does not match with {nameof(basketItem.ConfiguredCandle.LayerColors)} by id: {basketItem.ConfiguredCandle.LayerColors[0].Id}",
            result.Error);
    }

    [Fact]
    public void Create_IdWickNotMatch_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandleFilter = new ConfiguredCandleFilter
        {
            CandleId = configuredCandleFilter.CandleId,
            DecorId = configuredCandleFilter.DecorId,
            NumberOfLayerId = configuredCandleFilter.NumberOfLayerId,
            LayerColorIds = configuredCandleFilter.LayerColorIds,
            SmellId = configuredCandleFilter.SmellId,
            WickId = GenerateData.GenerateInvalidId(),
            Quantity = configuredCandleFilter.Quantity,
            FilterString = configuredCandleFilter.FilterString
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: basketItem.ConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: newConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotEqual(newConfiguredCandleFilter.WickId, basketItem.ConfiguredCandle.Wick.Id);
        Assert.Equal(
            $"{nameof(basketItem.ConfiguredCandle.Wick)} by id: {newConfiguredCandleFilter.WickId} " +
            $"does not match with {nameof(basketItem.ConfiguredCandle.Wick)} by id: {basketItem.ConfiguredCandle.Wick.Id}",
            result.Error);
    }

    [Fact]
    public void Create_SmellIdNotZeroAndSmellIsNull_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = basketItem.ConfiguredCandle.Candle,
            Decor = basketItem.ConfiguredCandle.Decor,
            LayerColors = basketItem.ConfiguredCandle.LayerColors,
            NumberOfLayer = basketItem.ConfiguredCandle.NumberOfLayer,
            Smell = null,
            Wick = basketItem.ConfiguredCandle.Wick
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: newConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: basketItem.ConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.True(basketItem.ConfiguredCandleFilter.DecorId != 0);
        Assert.True(newConfiguredCandle.Smell == null);
        Assert.Equal(
            $"{nameof(basketItem.ConfiguredCandle.Smell)} by id: {basketItem.ConfiguredCandleFilter.SmellId} is not found",
            result.Error);
    }

    [Fact]
    public void Create_SmellIdNotZeroAndSmellIdNotMatching_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = basketItem.ConfiguredCandle.Candle,
            Decor = basketItem.ConfiguredCandle.Decor,
            LayerColors = basketItem.ConfiguredCandle.LayerColors,
            NumberOfLayer = basketItem.ConfiguredCandle.NumberOfLayer,
            Smell = GenerateOrderData.GenerateSmell(),
            Wick = basketItem.ConfiguredCandle.Wick
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: newConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: basketItem.ConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.True(basketItem.ConfiguredCandleFilter.SmellId != 0);
        Assert.NotNull(newConfiguredCandle.Smell);
        Assert.Equal(
            $"{nameof(basketItem.ConfiguredCandle.Smell)} by id: {basketItem.ConfiguredCandleFilter.SmellId} " +
            $"does not match with {nameof(basketItem.ConfiguredCandle.Smell)} by id: {newConfiguredCandle.Smell?.Id}",
            result.Error);
    }

    [Fact]
    public void Create_SmellIdIsZeroAndSmellIsNotNull_ReturnFailure()
    {
        // Arrange
        var configuredCandle = GenerateOrderData.GenerateConfiguredCandle();

        var newConfiguredCandleFilter = new ConfiguredCandleFilter
        {
            CandleId  = configuredCandle.Candle.Id,
            DecorId  = configuredCandle.Decor?.Id,
            NumberOfLayerId  = configuredCandle.NumberOfLayer.Id,
            LayerColorIds  = configuredCandle.LayerColors.Select(l => l.Id).ToArray(),
            SmellId  = 0,
            WickId  = configuredCandle.Wick.Id,
            Quantity  = _faker.Random.Number(1, 100),
            FilterString  = _faker.Random.String()
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: configuredCandle,
            price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            configuredCandleFilter: newConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(configuredCandle.Smell);
        Assert.NotEqual(configuredCandle.Smell.Id, newConfiguredCandleFilter.SmellId);
        Assert.Equal(
            $"{nameof(configuredCandle.Smell)} by id: {configuredCandle.Smell.Id} is found, but it should not be in",
            result.Error);
    }

    [Fact]
    public void Create_SmellIdAndSmellIdAreNull_ReturnsSuccess()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = basketItem.ConfiguredCandle.Candle,
            Decor = basketItem.ConfiguredCandle.Decor,
            LayerColors = basketItem.ConfiguredCandle.LayerColors,
            NumberOfLayer = basketItem.ConfiguredCandle.NumberOfLayer,
            Smell = null,
            Wick = basketItem.ConfiguredCandle.Wick
        };

        var newConfiguredCandleFilter = new ConfiguredCandleFilter
        {
            CandleId  = basketItem.ConfiguredCandle.Candle.Id,
            DecorId  = basketItem.ConfiguredCandle.Decor?.Id,
            NumberOfLayerId  = basketItem.ConfiguredCandle.NumberOfLayer.Id,
            LayerColorIds  = basketItem.ConfiguredCandle.LayerColors.Select(l => l.Id).ToArray(),
            SmellId  = 0,
            WickId  = basketItem.ConfiguredCandle.Wick.Id,
            Quantity  = _faker.Random.Number(1, 100),
            FilterString  = _faker.Random.String()
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: newConfiguredCandle,
            price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            configuredCandleFilter: newConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(newConfiguredCandle.Smell);
        Assert.Equal(newConfiguredCandleFilter.SmellId, 0);
    }

    [Fact]
    public void Create_IdCandleAndIdWickNotMatch_ReturnFailure()
    {
        // Arrange
        var configuredCandleFilter = GenerateOrderData.GenerateConfiguredCandleFilter();
        var basketItem = GenerateOrderData.GenerateBasketItem(configuredCandleFilter);

        var newConfiguredCandleFilter = new ConfiguredCandleFilter
        {
            CandleId = _faker.Random.Number(0, 10000),
            DecorId = configuredCandleFilter.DecorId,
            NumberOfLayerId = configuredCandleFilter.NumberOfLayerId,
            LayerColorIds = configuredCandleFilter.LayerColorIds,
            SmellId = configuredCandleFilter.SmellId,
            WickId = _faker.Random.Number(0, 10000),
            Quantity = configuredCandleFilter.Quantity,
            FilterString = configuredCandleFilter.FilterString
        };

        // Act
        var result = BasketItem.Create(
            configuredCandle: basketItem.ConfiguredCandle,
            price: basketItem.Price,
            configuredCandleFilter: newConfiguredCandleFilter);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotEqual(newConfiguredCandleFilter.CandleId, basketItem.ConfiguredCandle.Candle.Id);
        Assert.NotEqual(newConfiguredCandleFilter.WickId, basketItem.ConfiguredCandle.Wick.Id);
        Assert.Contains(
            $"{nameof(basketItem.ConfiguredCandle.Wick)} by id: {newConfiguredCandleFilter.WickId} " +
            $"does not match with {nameof(basketItem.ConfiguredCandle.Wick)} by id: {basketItem.ConfiguredCandle.Wick.Id}",
            result.Error);
        Assert.Contains(
            $"{nameof(basketItem.ConfiguredCandle.Candle)} by id: {newConfiguredCandleFilter.CandleId} " +
            $"does not match with {nameof(basketItem.ConfiguredCandle.Candle)} by id: {basketItem.ConfiguredCandle.Candle.Id}",
           result.Error);
    }

    [Theory]
    [MemberData(nameof(GenerateTestDataForCreateValidParameters))]
    public void Compare_ValidParameters_ReturnsSuccess(
    ConfiguredCandle candleDetail,
    decimal price,
    ConfiguredCandleFilter configuredCandleFilter)
    {
        // Arrange
        var basketItem = BasketItem.Create(
            configuredCandle: candleDetail,
            price: price,
            configuredCandleFilter: configuredCandleFilter);

        // Act
        var result = basketItem.Value.Compare(basketItem.Value.ConfiguredCandle); ;

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Compare_IdCandleNotMatch_ReturnFailure()
    {
        // Arrange

        var basketItem = GenerateOrderData.GenerateBasketItem();

        var newConfiguredCandle = new ConfiguredCandle
        {
            Candle = GenerateOrderData.GenerateCandle(),
            Decor = basketItem.ConfiguredCandle.Decor,
            LayerColors = basketItem.ConfiguredCandle.LayerColors,
            NumberOfLayer = basketItem.ConfiguredCandle.NumberOfLayer,
            Smell = basketItem.ConfiguredCandle.Smell,
            Wick = basketItem.ConfiguredCandle.Wick
        };

        // Act

        var result = basketItem.Compare(newConfiguredCandle);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal($"{nameof(basketItem.ConfiguredCandle.Candle)} by id: {basketItem.ConfiguredCandle.Candle.Id} " +
            $"does not match with {nameof(basketItem.ConfiguredCandle.Candle)} by id: {newConfiguredCandle.Candle.Id}", 
            result.Error);
    }

    // TODO: Дописать тесты для метода Compare
}