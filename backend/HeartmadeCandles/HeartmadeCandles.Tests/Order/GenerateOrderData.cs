using Bogus;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.UnitTests.Order;

internal class GenerateOrderData
{
   private static readonly Faker _faker = new();

   public static Candle GenerateCandle(int id = 0)
    {
        return new Candle
        {
            Id = id == 0
                ? GenerateData.GenerateId()
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Candle.MaxTitleLength),
            Price = _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            WeightGrams = _faker.Random.Number(1, 10000),
            Images = new[]
            {
                new Image
                {
                    FileName = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Image.MaxAlternativeNameLength),
                    AlternativeName = _faker.Random.String()
                }
            }
        };
    }

    public static Decor GenerateDecor(int id = 0)
    {
        return new Decor
        {
            Id = id == 0
                ? GenerateData.GenerateId()
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Decor.MaxTitleLength),
            Price = _faker.Random.Number(1, 10000) * _faker.Random.Decimal()
        };
    }

    public static LayerColor GenerateLayerColor(int id = 0)
    {
        return new LayerColor
        {
            Id = id == 0
                ? GenerateData.GenerateId()
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.LayerColor.MaxTitleLength),
            PricePerGram = _faker.Random.Number(1, 10000) * _faker.Random.Decimal()
        };
    }

    public static NumberOfLayer GenerateNumberOfLayer(int number, int id = 0)
    {
        return new NumberOfLayer
        {
            Id = id == 0 
                ? GenerateData.GenerateId() 
                : id,
            Number = number
        };
    }

    public static Smell GenerateSmell(int id = 0)
    {
        return new Smell
        {
            Id = id == 0
                ? GenerateData.GenerateId()
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Smell.MaxTitleLength),
            Price = _faker.Random.Number(1, 10000) * _faker.Random.Decimal()
        };
    }

    public static Wick GenerateWick(int id = 0)
    {
        return new Wick
        {
            Id = id == 0
                ? GenerateData.GenerateId()
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Wick.MaxTitleLength),
            Price = _faker.Random.Number(1, 10000) * _faker.Random.Decimal()
        };
    }

    public static ConfiguredCandle GenerateConfiguredCandle(
        Candle? candle = null, 
        Decor? decor = null, 
        List<LayerColor>? layerColors = null, 
        NumberOfLayer? numberOfLayer = null, 
        Smell? smell = null, 
        Wick? wick = null)
    {
        if (candle == null)
        {
            candle = GenerateCandle();
        }

        if (numberOfLayer == null)
        {
            numberOfLayer = GenerateNumberOfLayer(_faker.Random.Number(1, 100));
        }

        if (layerColors == null)
        {
            layerColors = new List<LayerColor>();
            for (var i = 0; i < numberOfLayer.Number; i++)
                layerColors.Add(GenerateLayerColor());
        }

        if (decor == null)
        {
            decor = GenerateDecor();
        }

        if (smell == null)
        {
            smell = GenerateSmell();
        }

        if (wick == null)
        {
            wick = GenerateWick();
        }

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

        var basketItem = BasketItem.Create(
            configuredCandle,
            configuredCandleFilter.Quantity,
            configuredCandleFilter);

        return basketItem.Value;
    }

    public static BasketItem GenerateBasketItem() =>  GenerateBasketItem(GenerateConfiguredCandleFilter());

    public static Basket GenerateBasket()
    {
        return new Basket 
        {
            Id = GenerateData.GenerateStringId(),
            Items = Enumerable
                .Range(0, _faker.Random.Number(1, 100))
                .Select(_ => GenerateBasketItem())
                .ToArray(),
            FilterString = _faker.Random.String()
        };
    }

    public static Basket GenerateBasket(ConfiguredCandleBasket configuredCandleBasket)
    {
        return new Basket 
        {
            Id = GenerateData.GenerateStringId(),
            Items = configuredCandleBasket.ConfiguredCandleFilters
                .Select(x => GenerateBasketItem(x))
                .ToArray(),
            FilterString = configuredCandleBasket.ConfiguredCandleFiltersString
        };
    }

    public static ConfiguredCandleBasket GenerateConfiguredCandleBasket()
    {
        return new ConfiguredCandleBasket
        {
            ConfiguredCandleFilters = Enumerable
                .Range(0, _faker.Random.Number(1, 100))
                .Select(_ => GenerateConfiguredCandleFilter())
                .ToArray(),
            ConfiguredCandleFiltersString = _faker.Random.String()
        };
    }

    private static User GenerateUser()
    {
        var user = _faker.Person;

        return new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Email = user.Email
        };
    }

    private static Feedback GenerateFeedback()
    {
        return new Feedback
        {
            TypeFeedback = _faker.Random.String(),
            UserName = _faker.Random.String()
        };
    }

    public static HeartmadeCandles.Order.Core.Models.Order GenerateOrder()
    {
        var basket = GenerateBasket();

        return new HeartmadeCandles.Order.Core.Models.Order
        {
            Id = GenerateData.GenerateStringId(),
            BasketId = basket.Id ?? GenerateData.GenerateStringId(),
            Basket = basket,
            User = GenerateUser(),
            Feedback = GenerateFeedback(),
            Status = OrderStatus.Created
        };
    }
}