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

    public static Candle GenerateCandle()
    {
        return new Candle
        {
            Id = _faker.Random.Number(1, 10000),
            Title = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Candle.MaxTitleLength),
            Description = _faker.Random.String(1, HeartmadeCandles.Admin.Core.Models.Candle.MaxDescriptionLength),
            Price =  _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            WeightGrams = _faker.Random.Number(1, 10000),
            Images = GenerateImages(),
        };
    }

    public static Decor GenerateDecor()
    {
        return new Decor
        {  
            Id = _faker.Random.Number(1, 10000),
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

    public static LayerColor GenerateLayerColor()
    {
        return new LayerColor
        {
            Id = _faker.Random.Number(1, 10000),
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

    public static NumberOfLayer GenerateNumberOfLayer()
    {
        return new NumberOfLayer
        {
            Id = _faker.Random.Number(1, 10000),
            Number = _faker.Random.Number(1, 10000)
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

    public static Smell GenerateSmell()
    {
        return new Smell
        {
            Id = _faker.Random.Number(1, 10000),
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

    public static Wick GenerateWick()
    {
        return new Wick
        {
            Id = _faker.Random.Number(1, 10000),
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

    public static TypeCandle GenerateTypeCandle()
    {
        return new TypeCandle
        {
            Id = _faker.Random.Number(1, 10000),
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
