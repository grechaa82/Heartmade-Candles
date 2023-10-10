using Bogus;
using HeartmadeCandles.Order.Core.Models;
using Image = HeartmadeCandles.Admin.Core.Models.Image;

namespace HeartmadeCandles.UnitTests.Order;

internal class GenerateOrderData
{
    private static readonly Faker _faker = new();

    public static Candle GenerateCandle()
    {
        return new Candle(
            _faker.Random.Number(1, 10000),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Candle.MaxTitleLength),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Candle.MaxDescriptionLength),
            _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            _faker.Random.Number(1, 10000),
            new[]
            {
                new HeartmadeCandles.Order.Core.Models.Image(
                    _faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String())
            }
        );
    }

    public static Decor GenerateDecor()
    {
        return new Decor(
            _faker.Random.Number(1, 10000),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Decor.MaxTitleLength),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Decor.MaxDescriptionLength),
            _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            new[]
            {
                new HeartmadeCandles.Order.Core.Models.Image(
                    _faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String())
            }
        );
    }

    public static LayerColor GenerateLayerColor()
    {
        return new LayerColor
        (
            _faker.Random.Number(1, 10000),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.LayerColor.MaxTitleLength),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.LayerColor.MaxDescriptionLength),
            _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            new[]
            {
                new HeartmadeCandles.Order.Core.Models.Image(
                    _faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String())
            }
        );
    }

    public static NumberOfLayer GenerateNumberOfLayer()
    {
        return new NumberOfLayer
        (
            _faker.Random.Number(1, 10000),
            _faker.Random.Number(1, 10000)
        );
    }

    public static Smell GenerateSmell()
    {
        return new Smell
        (
            _faker.Random.Number(1, 10000),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Smell.MaxTitleLength),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Smell.MaxDescriptionLength),
            _faker.Random.Number(1, 10000) * _faker.Random.Decimal()
        );
    }

    public static Wick GenerateWick()
    {
        return new Wick
        (
            _faker.Random.Number(1, 10000),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Wick.MaxTitleLength),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Wick.MaxDescriptionLength),
            _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            new[]
            {
                new HeartmadeCandles.Order.Core.Models.Image(
                    _faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String())
            }
        );
    }

    public static CandleDetail GenerateCandleDetail()
    {
        var candle = GenerateCandle();
        var numberOfLayer = GenerateNumberOfLayer();
        var layerColors = new List<LayerColor>();
        for (var i = 0; i < numberOfLayer.Number; i++) layerColors.Add(GenerateLayerColor());
        var decor = GenerateDecor();
        var smell = GenerateSmell();
        var wick = GenerateWick();

        return new CandleDetail(candle, decor, layerColors.ToArray(), numberOfLayer, smell, wick);
    }

    public static OrderItemFilter GenerateOrderItemFilter(CandleDetail candleDetail, int quantity)
    {
        var candleId = candleDetail.Candle.Id;
        var decorId = candleDetail.Decor?.Id ?? 0;
        var numberOfLayerId = candleDetail.NumberOfLayer.Id;
        var layerColorIds = candleDetail.LayerColors.Select(l => l.Id).ToArray();
        var smellId = candleDetail.Smell?.Id ?? 0;
        var wickId = candleDetail.Wick.Id;

        return new OrderItemFilter(candleId, decorId, numberOfLayerId, layerColorIds, smellId, wickId, quantity);
    }
}