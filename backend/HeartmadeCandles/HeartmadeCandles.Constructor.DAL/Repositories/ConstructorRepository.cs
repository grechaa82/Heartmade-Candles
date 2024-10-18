using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Constructor.Core.Models;
using HeartmadeCandles.Constructor.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Constructor.DAL.Repositories;

public class ConstructorRepository : IConstructorRepository
{
    private readonly ConstructorDbContext _context;

    public ConstructorRepository(ConstructorDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CandleTypeWithCandles[]>> GetCandles()
    {
        var items = await _context.Candle
            .AsNoTracking()
            .Where(c => c.IsActive)
            .GroupBy(c => c.TypeCandle.Title)
            .ToArrayAsync();

        var result = items.Select(
                c => new CandleTypeWithCandles
                {
                    Type = c.Key,
                    Candles = c.OrderBy(candle => candle.Id)
                        .Select(candle => MapToCandle(candle))
                        .Take(15)
                        .ToArray()
                })
            .ToArray();

        return Result.Success(result);
    }


    public async Task<(Maybe<Candle[]>, long)> GetCandlesByType(TypeCandle typeCandle, int pageSize, int pageIndex)
    {
        var candlesQuery = _context.Candle
            .AsNoTracking()
            .Where(c => c.IsActive && c.TypeCandle.Id == typeCandle.Id);
        
        var totalCount = await candlesQuery.LongCountAsync();

        var items = await candlesQuery
            .OrderBy(c => c.Id)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToArrayAsync();

        if (!items.Any())
        {
            return (Maybe<Candle[]>.None, totalCount);
        }

        var result = items
            .Select(MapToCandle)
            .ToArray();
        
        return (result, totalCount);
    }

    public async Task<Maybe<CandleDetail>> GetCandleById(int candleId)
    {
        var candleDetailEntity = await _context.Candle
            .AsNoTracking()
            .Include(t => t.TypeCandle)
            .Include(cd => cd.CandleDecor.Where(d => d.Decor.IsActive))
            .ThenInclude(d => d.Decor)
            .Include(cl => cl.CandleLayerColor.Where(l => l.LayerColor.IsActive))
            .ThenInclude(l => l.LayerColor)
            .Include(cn => cn.CandleNumberOfLayer)
            .ThenInclude(n => n.NumberOfLayer)
            .Include(cs => cs.CandleSmell.Where(s => s.Smell.IsActive))
            .ThenInclude(s => s.Smell)
            .Include(cw => cw.CandleWick.Where(w => w.Wick.IsActive))
            .ThenInclude(w => w.Wick)
            .FirstOrDefaultAsync(c => c.Id == candleId && c.IsActive);

        if (candleDetailEntity == null)
        {
            return Maybe.None;
        }

        var candle = MapToCandle(candleDetailEntity);

        var decors = candleDetailEntity.CandleDecor
            .Select(cd => MapToDecor(cd.Decor))
            .ToArray();

        var layerColors = candleDetailEntity.CandleLayerColor
            .Select(cl => MapToLayerColor(cl.LayerColor))
            .ToArray();

        var numberOfLayers = candleDetailEntity.CandleNumberOfLayer
            .Select(cn => MapToNumberOfLayer(cn.NumberOfLayer))
            .ToArray();

        var smells = candleDetailEntity.CandleSmell
            .Select(cs => MapToSmell(cs.Smell))
            .ToArray();

        var wicks = candleDetailEntity.CandleWick
            .Select(cw => MapToWick(cw.Wick))
            .ToArray();

        return new CandleDetail
        {
            Candle = candle,
            Decors = decors,
            LayerColors = layerColors,
            NumberOfLayers = numberOfLayers,
            Smells = smells,
            Wicks = wicks
        };
    }

    public async Task<Result<CandleDetail>> GetCandleByFilter(CandleDetailFilter candleDetailFilter)
    {
        var candleDetailEntity = await _context.Candle
            .AsNoTracking()
            .Include(t => t.TypeCandle)
            .Include(cd => cd.CandleDecor.Where(d => d.Decor.Id == candleDetailFilter.DecorId && d.Decor.IsActive))
            .ThenInclude(d => d.Decor)
            .Include(
                cl => cl.CandleLayerColor.Where(
                    l => candleDetailFilter.LayerColorIds.Contains(l.LayerColor.Id) && l.LayerColor.IsActive))
            .ThenInclude(l => l.LayerColor)
            .Include(cn => cn.CandleNumberOfLayer.Where(n => n.NumberOfLayer.Id == candleDetailFilter.NumberOfLayerId))
            .ThenInclude(n => n.NumberOfLayer)
            .Include(cs => cs.CandleSmell.Where(s => s.Smell.Id == candleDetailFilter.SmellId && s.Smell.IsActive))
            .ThenInclude(s => s.Smell)
            .Include(cw => cw.CandleWick.Where(w => w.Wick.Id == candleDetailFilter.WickId && w.Wick.IsActive))
            .ThenInclude(w => w.Wick)
            .FirstOrDefaultAsync(c => c.Id == candleDetailFilter.CandleId && c.IsActive);

        if (candleDetailEntity == null)
        {
            return Result.Failure<CandleDetail>($"Candle by id: {candleDetailFilter.CandleId} does not exist");
        }

        var candle = MapToCandle(candleDetailEntity);

        var decors = candleDetailEntity.CandleDecor
            .Select(cd => MapToDecor(cd.Decor))
            .ToArray();
        var layerColorsDictionary =
            candleDetailEntity.CandleLayerColor.ToDictionary(x => x.LayerColor.Id, x => x.LayerColor);
        var layerColors = candleDetailFilter.LayerColorIds
            .Select(
                x => layerColorsDictionary.TryGetValue(x, out var layerColor) ? MapToLayerColor(layerColor) : null)
            .Where(x => x != null)
            .ToArray();
        var numberOfLayers = candleDetailEntity.CandleNumberOfLayer
            .Select(cn => MapToNumberOfLayer(cn.NumberOfLayer))
            .ToArray();
        var smells = candleDetailEntity.CandleSmell
            .Select(cs => MapToSmell(cs.Smell))
            .ToArray();
        var wicks = candleDetailEntity.CandleWick
            .Select(cw => MapToWick(cw.Wick))
            .ToArray();

        if (!numberOfLayers.Any())
        {
            return Result.Failure<CandleDetail>(
                $"NumberOfLayer by id: {candleDetailFilter.NumberOfLayerId} does not exist");
        }

        if (!layerColors.Any())
        {
            return Result.Failure<CandleDetail>("LayerColors does not exist");

        }

        if (!wicks.Any())
        {
            return Result.Failure<CandleDetail>($"Wick by id: {candleDetailFilter.WickId} does not exist");
        }

        return new CandleDetail
        {
            Candle = candle,
            Decors = decors,
            LayerColors = layerColors!,
            NumberOfLayers = numberOfLayers,
            Smells = smells,
            Wicks = wicks
        };
    }

    public async Task<Maybe<TypeCandle>> GetTypeCandleByTitle(string typeCandle)
    {
        var item = await _context.TypeCandle
           .AsNoTracking()
           .FirstOrDefaultAsync(c => c.Title == typeCandle);

        if (item == null)
        {
            return Maybe<TypeCandle>.None;
        }

        var result = MapToTypeCandle(item);

        return result;
    }

    private Candle MapToCandle(CandleEntity candleEntity)
    {
        return new Candle
        {
            Id = candleEntity.Id,
            Title = candleEntity.Title,
            Description = candleEntity.Description,
            Price = candleEntity.Price,
            WeightGrams = candleEntity.WeightGrams,
            Images = MapToImage(candleEntity.Images)
        };
    }

    private TypeCandle MapToTypeCandle(TypeCandleEntity typeCandleEntity)
    {
        return new TypeCandle
        {
            Id = typeCandleEntity.Id,
            Title = typeCandleEntity.Title
        };
    }

    private Decor MapToDecor(DecorEntity decorEntity)
    {
        return new Decor
        {
            Id = decorEntity.Id,
            Title = decorEntity.Title,
            Description = decorEntity.Description,
            Price = decorEntity.Price,
            Images = MapToImage(decorEntity.Images)
        };
    }

    private LayerColor MapToLayerColor(LayerColorEntity layerColorEntity)
    {
        return new LayerColor
        {
            Id = layerColorEntity.Id,
            Title = layerColorEntity.Title,
            Description = layerColorEntity.Description,
            PricePerGram = layerColorEntity.PricePerGram,
            Images = MapToImage(layerColorEntity.Images)
        };
    }

    private NumberOfLayer MapToNumberOfLayer(NumberOfLayerEntity numberOfLayerEntity)
    {
        return new NumberOfLayer
        {
            Id = numberOfLayerEntity.Id,
            Number = numberOfLayerEntity.Number
        };
    }

    private Smell MapToSmell(SmellEntity smellEntity)
    {
        return new Smell
        {
            Id = smellEntity.Id,
            Title = smellEntity.Title,
            Description = smellEntity.Description,
            Price = smellEntity.Price
        };
    }

    public Wick MapToWick(WickEntity wickEntity)
    {
        return new Wick
        {
            Id = wickEntity.Id,
            Title = wickEntity.Title,
            Description = wickEntity.Description,
            Price = wickEntity.Price,
            Images = MapToImage(wickEntity.Images)
        };
    }

    private Image[] MapToImage(ImageEntity[] imageEntities)
    {
        var images = new List<Image>();

        foreach (var imageEntity in imageEntities)
        {
            var image = new Image
            {
                FileName = imageEntity.FileName,
                AlternativeName = imageEntity.AlternativeName
            };

            images.Add(image);
        }

        return images.ToArray();
    }
}