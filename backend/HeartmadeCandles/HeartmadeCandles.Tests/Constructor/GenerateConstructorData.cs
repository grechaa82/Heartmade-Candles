using Bogus;
using HeartmadeCandles.Constructor.Core.Models;

namespace HeartmadeCandles.UnitTests;

public class GenerateConstructorData
{
    private static readonly Faker _faker = new();

    public static CandleDetail GenerateCandleDetail()
    {
        return new CandleDetail
        {
            Candle = GenerateCandle(),
            Decors = GenerateDecors(_faker.Random.Number(1, 100)),
            LayerColors = GenerateLayerColors(_faker.Random.Number(1, 100)),
            NumberOfLayers = GenerateNumberOfLayers(_faker.Random.Number(1, 100)),
            Smells = GenerateSmells(_faker.Random.Number(1, 100)),
            Wicks = GenerateWicks(_faker.Random.Number(1, 100)),
        };
    }
    
    public static CandleDetail GenerateCandleDetail(HeartmadeCandles.Order.Core.Models.ConfiguredCandleFilter configuredCandleFilter)
    {
        return new CandleDetail
        {
            Candle = GenerateCandle(configuredCandleFilter.CandleId),
            Decors = new Decor[] { GenerateDecor(configuredCandleFilter.DecorId ?? 0) },
            LayerColors = configuredCandleFilter.LayerColorIds
                .Select(x => GenerateLayerColor(x))
                .ToArray(),
            NumberOfLayers = new NumberOfLayer[] 
            {
                GenerateNumberOfLayer(
                    configuredCandleFilter.NumberOfLayerId, 
                    configuredCandleFilter.LayerColorIds.Length)
            },                
            Smells = new Smell[] { GenerateSmell(configuredCandleFilter.SmellId ?? 0) },
            Wicks = new Wick[] { GenerateWick(configuredCandleFilter.WickId) },
        };
    }

    public static Candle GenerateCandle(int id = 0)
    {
        return new Candle
        {
            Id = id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Candle.MaxTitleLength),
            Description = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Candle.MaxDescriptionLength),
            Price =  _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            WeightGrams = _faker.Random.Number(1, 10000),
            Images = GenerateImages(),
        };
    }

    public static Decor GenerateDecor(int id = 0)
    {
        return new Decor
        {  
            Id = id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Decor.MaxTitleLength),
            Description = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Decor.MaxDescriptionLength),
            Price = _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            Images = GenerateImages(),
        };
    }

    public static Decor[] GenerateDecors(int count)
    {
        var decors = new Decor[count];

        for(int i = 0; i < count; i++)
        {
            decors[i] = GenerateDecor();
        }

        return decors;
    }

    public static LayerColor GenerateLayerColor(int id = 0)
    {
        return new LayerColor
        {
            Id = id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.LayerColor.MaxTitleLength),
            Description = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.LayerColor.MaxDescriptionLength),
            PricePerGram = _faker.Random.Number(1, 10000),
            Images = GenerateImages()
        };
    }

    public static LayerColor[] GenerateLayerColors(int count)
    {
        var layerColors = new LayerColor[count];

        for(int i = 0; i < count; i++)
        {
            layerColors[i] = GenerateLayerColor();
        }

        return layerColors;
    }

    public static NumberOfLayer GenerateNumberOfLayer(int id = 0, int number = 0)
    {
        return new NumberOfLayer
        {
            Id = id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            Number = number == 0
                ? _faker.Random.Number(1, 10000)
                : number
        };
    }

    public static NumberOfLayer[] GenerateNumberOfLayers(int count)
    {
        var numberOfLayers = new NumberOfLayer[count];

        for(int i = 0; i < count; i++)
        {
            numberOfLayers[i] = GenerateNumberOfLayer();
        }

        return numberOfLayers;
    }

    public static Smell GenerateSmell(int id = 0)
    {
        return new Smell
        {
            Id = id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Smell.MaxTitleLength),
            Description = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Smell.MaxDescriptionLength),
            Price = _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
        };
    }

    public static Smell[] GenerateSmells(int count)
    {
        var smells = new Smell[count];

        for(int i = 0; i < count; i++)
        {
            smells[i] = GenerateSmell();
        }

        return smells;
    }

    public static Wick GenerateWick(int id = 0)
    {
        return new Wick
        {
            Id = id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Wick.MaxTitleLength),
            Description = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Wick.MaxDescriptionLength),
            Price = _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            Images = GenerateImages()
        };
    }

    public static Wick[] GenerateWicks(int count)
    {
        var wicks = new Wick[count];

        for(int i = 0; i < count; i++)
        {
            wicks[i] = GenerateWick();
        }

        return wicks;
    }

    public static TypeCandle GenerateTypeCandle(int id = 0)
    {
        return new TypeCandle
        {
            Id = id == 0
                ? _faker.Random.Number(1, 10000)
                : id,
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.TypeCandle.MaxTitleLength)
        };
    }

    public static Image[] GenerateImages()
    {
        return new[]
        {
            new Image
            {
                FileName = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Image.MaxAlternativeNameLength),
                AlternativeName = _faker.Random.String()
            }
        };
    }
}
