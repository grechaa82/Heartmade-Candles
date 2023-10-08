using Bogus;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.UnitTests.Order.Core.Models;

public class OrderItemTest
{
    private static readonly Faker _faker = new();

    [Theory]
    [MemberData(nameof(GenerateData))]
    public void Create_ValidParameters_ReturnsSuccess(CandleDetail candleDetail, int quantity,
        OrderItemFilter orderItemFilter)
    {
        // Arrange

        // Act
        var result = OrderItem.Create(candleDetail, quantity, orderItemFilter);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Price > 0);
        Assert.Equal(quantity, result.Value.Quantity);
    }

    public static IEnumerable<object[]> GenerateData()
    {
        for (var i = 0; i < 100; i++)
        {
            var candleDetail = GenerateCandleDetail();
            var quantity = _faker.Random.Number(1, 10000);

            yield return new object[]
            {
                candleDetail,
                quantity,
                GenerateOrderItemFilter(candleDetail, quantity)
            };
        }
    }

    private static CandleDetail GenerateCandleDetail()
    {
        var candle = GenerateOrderData.GenerateCandle();
        var numberOfLayer = GenerateOrderData.GenerateNumberOfLayer();
        var layerColors = new List<LayerColor>();
        for (var i = 0; i < numberOfLayer.Number; i++) layerColors.Add(GenerateOrderData.GenerateLayerColor());
        var decor = GenerateOrderData.GenerateDecor();
        var smell = GenerateOrderData.GenerateSmell();
        var wick = GenerateOrderData.GenerateWick();


        return new CandleDetail(candle, decor, layerColors.ToArray(), numberOfLayer, smell, wick);
    }

    private static OrderItemFilter GenerateOrderItemFilter(CandleDetail candleDetail, int quantity)
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