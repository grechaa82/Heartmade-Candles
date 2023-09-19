using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models;

public class CandleDetail
{
    private CandleDetail(
        Candle candle,
        List<Decor> decors,
        List<LayerColor> layerColors,
        List<NumberOfLayer> numberOfLayers,
        List<Smell> smells,
        List<Wick> wicks)
    {
        Candle = candle;
        Decors = decors;
        LayerColors = layerColors;
        NumberOfLayers = numberOfLayers;
        Smells = smells;
        Wicks = wicks;
    }

    public Candle Candle { get; }

    public List<Decor> Decors { get; }

    public List<LayerColor> LayerColors { get; }

    public List<NumberOfLayer> NumberOfLayers { get; }

    public List<Smell> Smells { get; }

    public List<Wick> Wicks { get; }

    public static Result<CandleDetail> Create(
        Candle candle,
        List<Decor> decors,
        List<LayerColor> layerColors,
        List<NumberOfLayer> numberOfLayers,
        List<Smell> smells,
        List<Wick> wicks)
    {
        var result = Result.Success();

        if (candle == null)
        {
            result = Result.Combine(
                result,
                Result.Failure<CandleDetail>($"'{nameof(candle)}' cannot be null"));
        }

        if (decors == null)
        {
            result = Result.Combine(
                result,
                Result.Failure<CandleDetail>($"'{nameof(decors)}' cannot be null"));
        }

        if (layerColors == null)
        {
            result = Result.Combine(
                result,
                Result.Failure<CandleDetail>($"'{nameof(layerColors)}' cannot be null"));
        }

        if (numberOfLayers == null)
        {
            result = Result.Combine(
                result,
                Result.Failure<CandleDetail>($"'{nameof(numberOfLayers)}' cannot be null"));
        }

        if (smells == null)
        {
            result = Result.Combine(
                result,
                Result.Failure<CandleDetail>($"'{nameof(smells)}' cannot be null"));
        }

        if (wicks == null)
        {
            result = Result.Combine(
                result,
                Result.Failure<CandleDetail>($"'{nameof(wicks)}' cannot be null"));
        }

        if (result.IsFailure)
        {
            return Result.Failure<CandleDetail>(result.Error);
        }

        var candleDetail = new CandleDetail(
            candle,
            decors,
            layerColors,
            numberOfLayers,
            smells,
            wicks);

        return Result.Success(candleDetail);
    }
}