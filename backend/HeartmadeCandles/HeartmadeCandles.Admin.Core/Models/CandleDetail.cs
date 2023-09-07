using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class CandleDetail
    {
        private Candle _candle;
        private List<Decor> _decors;
        private List<LayerColor> _layerColors;
        private List<NumberOfLayer> _numberOfLayers;
        private List<Smell> _smells;
        private List<Wick> _wicks;

        private CandleDetail(
            Candle candle,
            List<Decor> decors,
            List<LayerColor> layerColors,
            List<NumberOfLayer> numberOfLayers,
            List<Smell> smells,
            List<Wick> wicks)
        {
            _candle = candle;
            _decors = decors;
            _layerColors = layerColors;
            _numberOfLayers = numberOfLayers;
            _smells = smells;
            _wicks = wicks;
        }

        public Candle Candle { get => _candle; }
        public List<Decor> Decors { get => _decors; }
        public List<LayerColor> LayerColors { get => _layerColors; }
        public List<NumberOfLayer> NumberOfLayers { get => _numberOfLayers; }
        public List<Smell> Smells { get => _smells; }
        public List<Wick> Wicks { get => _wicks; }

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
}
