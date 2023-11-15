using Bogus;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.UnitTests.Order;

internal class GenerateOrderData
{
    private static readonly Faker _faker = new();

   public static Candle GenerateCandle(int id = 0)
    {
        return new Candle(
            id: id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            title: _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Candle.MaxTitleLength),
            price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            weightGrams: _faker.Random.Number(1, 10000),
            images: new[]
            {
                new Image(
                    _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Image.MaxAlternativeNameLength),
                    _faker.Random.String())
            });
    }

    public static Decor GenerateDecor(int id = 0)
    {
        return new Decor(
            id: id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            title: _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Decor.MaxTitleLength),
            price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal());
    }

    public static LayerColor GenerateLayerColor(int id = 0)
    {
        return new LayerColor(
            id:id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            title: _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.LayerColor.MaxTitleLength),
            pricePerGram: _faker.Random.Number(1, 10000) * _faker.Random.Decimal());
    }

    public static NumberOfLayer GenerateNumberOfLayer(int number, int id = 0)
    {
        return new NumberOfLayer(
            id: id == 0 ? _faker.Random.Number(1, 10000) : id, 
            number: number);
    }

    public static Smell GenerateSmell(int id = 0)
    {
        return new Smell(
            id: id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            title: _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Smell.MaxTitleLength),
            price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal());
    }

    public static Wick GenerateWick(int id = 0)
    {
        return new Wick(
            id: id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            title: _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Wick.MaxTitleLength),
            price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal());
    }

    public static ConfiguredCandle GenerateConfiguredCandle()
    {
        var candle = GenerateCandle();
        var numberOfLayer = GenerateNumberOfLayer(_faker.Random.Number(1, 100));
        var layerColors = new List<LayerColor>();

        for (var i = 0; i < numberOfLayer.Number; i++)
            layerColors.Add(GenerateLayerColor());

        var decor = GenerateDecor();
        var smell = GenerateSmell();
        var wick = GenerateWick();

        return new ConfiguredCandle
        {
            Candle = candle,
            Decor = decor,
            LayerColors = layerColors.ToArray(),
            NumberOfLayer = numberOfLayer,
            Smell = smell,
            Wick = wick
        };
    }

    public static ConfiguredCandleFilter GenerateConfiguredCandleFilter(ConfiguredCandle candleDetail, int quantity)
    {
        var candleId = candleDetail.Candle.Id;
        var decorId = candleDetail.Decor?.Id ?? 0;
        var numberOfLayerId = candleDetail.NumberOfLayer.Id;
        var layerColorIds = candleDetail.LayerColors.Select(l => l.Id).ToArray();
        var smellId = candleDetail.Smell?.Id ?? 0;
        var wickId = candleDetail.Wick.Id;

        return new ConfiguredCandleFilter
        {
            CandleId  = candleId,
            DecorId  = decorId,
            NumberOfLayerId  = numberOfLayerId,
            LayerColorIds  = layerColorIds,
            SmellId  = smellId,
            WickId  = wickId,
            Quantity  = quantity,
            FilterString  = _faker.Random.String()
        };
    }

    public static ConfiguredCandleFilter GenerateConfiguredCandleFilter()
    {
        var candleDetail = GenerateConfiguredCandle();
        var quantity = _faker.Random.Number(1, 100);
        var candleId = candleDetail.Candle.Id;
        var decorId = candleDetail.Decor?.Id ?? 0;
        var numberOfLayerId = candleDetail.NumberOfLayer.Id;
        var layerColorIds = candleDetail.LayerColors.Select(l => l.Id).ToArray();
        var smellId = candleDetail.Smell?.Id ?? 0;
        var wickId = candleDetail.Wick.Id;

        return new ConfiguredCandleFilter
        {
            CandleId  = candleId,
            DecorId  = decorId,
            NumberOfLayerId  = numberOfLayerId,
            LayerColorIds  = layerColorIds,
            SmellId  = smellId,
            WickId  = wickId,
            Quantity  = quantity,
            FilterString  = _faker.Random.String()
        };
    }

    public static BasketItem GenerateBasketItem(ConfiguredCandleFilter configuredCandleFilter)
    {
        var candle = GenerateCandle(configuredCandleFilter.CandleId);
        var decor = configuredCandleFilter.DecorId.HasValue ? GenerateDecor((int)configuredCandleFilter.DecorId) : null;
        var numberOfLayer = GenerateNumberOfLayer(configuredCandleFilter.LayerColorIds.Length, configuredCandleFilter.NumberOfLayerId);
        
        var layerColors = new List<LayerColor>();
        for (var i = 0; i < numberOfLayer.Number; i++)
            layerColors.Add(GenerateLayerColor(configuredCandleFilter.LayerColorIds[i]));

        var smell = configuredCandleFilter.SmellId.HasValue ? GenerateSmell((int)configuredCandleFilter.SmellId) : null;
        var wick = GenerateWick(configuredCandleFilter.WickId);

        var configuredCandle = new ConfiguredCandle
        {
            Candle = candle,
            Decor = decor,
            LayerColors = layerColors.ToArray(),
            NumberOfLayer = numberOfLayer,
            Smell = smell,
            Wick = wick
        };

        var orderItem = BasketItem.Create(
            configuredCandle,
            configuredCandleFilter.Quantity,
            configuredCandleFilter);

        return orderItem.Value;
    }
}