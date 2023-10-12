using Bogus;
using HeartmadeCandles.Order.Core.Models;
using Image = HeartmadeCandles.Admin.Core.Models.Image;

namespace HeartmadeCandles.UnitTests.Order;

internal class GenerateOrderData
{
    private static readonly Faker _faker = new();

    public static Candle GenerateCandle(int id = 0)
    {
        return new Candle(
            id == 0 ? _faker.Random.Number(1, 10000) : id,
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

    public static Decor GenerateDecor(int id = 0)
    {
        return new Decor(
            id == 0 ? _faker.Random.Number(1, 10000) : id,
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

    public static LayerColor GenerateLayerColor(int id = 0)
    {
        return new LayerColor
        (
            id == 0 ? _faker.Random.Number(1, 10000) : id,
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

    public static NumberOfLayer GenerateNumberOfLayer(int id = 0)
    {
        var number = id == 0 ? _faker.Random.Number(1, 10000) : id;
        return new NumberOfLayer
        (
            number, number
        );
    }

    public static Smell GenerateSmell(int id = 0)
    {
        return new Smell
        (
            id == 0 ? _faker.Random.Number(1, 10000) : id,
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Smell.MaxTitleLength),
            _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Smell.MaxDescriptionLength),
            _faker.Random.Number(1, 10000) * _faker.Random.Decimal()
        );
    }

    public static Wick GenerateWick(int id = 0)
    {
        return new Wick
        (
            id == 0 ? _faker.Random.Number(1, 10000) : id,
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

    public static OrderItemFilter GenerateOrderItemFilter()
    {
        var candleDetail = GenerateCandleDetail();
        var quantity = _faker.Random.Number(1, 100);

        var candleId = candleDetail.Candle.Id;
        var decorId = candleDetail.Decor?.Id ?? 0;
        var numberOfLayerId = candleDetail.NumberOfLayer.Id;
        var layerColorIds = candleDetail.LayerColors.Select(l => l.Id).ToArray();
        var smellId = candleDetail.Smell?.Id ?? 0;
        var wickId = candleDetail.Wick.Id;

        return new OrderItemFilter(candleId, decorId, numberOfLayerId, layerColorIds, smellId, wickId, quantity);
    }

    public static OrderItem GenerateOrderItem(OrderItemFilter orderItemFilter)
    {
        var candle = GenerateCandle(orderItemFilter.CandleId);
        var decor = GenerateDecor(orderItemFilter.DecorId);
        var layerColors = new List<LayerColor>();
        for (var i = 0; i < orderItemFilter.LayerColorIds.Length; i++)
            layerColors.Add(GenerateLayerColor(orderItemFilter.LayerColorIds[i]));
        var numberOfLayer = GenerateNumberOfLayer(orderItemFilter.NumberOfLayerId);
        var smell = GenerateSmell(orderItemFilter.SmellId);
        var wick = GenerateWick(orderItemFilter.WickId);

        var candleDetail = new CandleDetail(candle, decor, layerColors.ToArray(), numberOfLayer, smell, wick);

        var orderItem = OrderItem.Create(candleDetail, orderItemFilter.Quantity, orderItemFilter);

        return orderItem.Value;
    }
}