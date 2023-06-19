using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services
{
    public class CandleService : ICandleService
    {
        private readonly ICandleRepository _candleRepository;
        private readonly IDecorRepository _decorRepository;
        private readonly ILayerColorRepository _layerColorRepository;
        private readonly INumberOfLayerRepository _numberOfLayerRepository;
        private readonly ISmellRepository _smellRepository;
        private readonly ITypeCandleRepository _typeCandleRepository;
        private readonly IWickRepository _wickRepository;

        public CandleService(
            ICandleRepository candleRepository, 
            IDecorRepository decorRepository, 
            ILayerColorRepository layerColorRepository, 
            INumberOfLayerRepository numberOfLayerRepository, 
            ISmellRepository smellRepository, 
            ITypeCandleRepository typeCandleRepository, 
            IWickRepository wickRepository)
        {
            _candleRepository = candleRepository;
            _decorRepository = decorRepository;
            _layerColorRepository = layerColorRepository;
            _numberOfLayerRepository = numberOfLayerRepository;
            _smellRepository = smellRepository;
            _typeCandleRepository = typeCandleRepository;
            _wickRepository = wickRepository;
        }

        public async Task<Candle[]> GetAll()
        {
            return await _candleRepository.GetAll();
        }

        public async Task<CandleDetail> Get(int id)
        {
            return await _candleRepository.Get(id);
        }

        public async Task Create(Candle candle)
        {
            await _candleRepository.Create(candle);
        }

        public async Task Update(Candle candle)
        {
            await _candleRepository.Update(candle);
        }

        public async Task Delete(int id)
        {
            await _candleRepository.Delete(id);
        }

        public async Task<Result> UpdateDecor(int id, int[] ids)
        {
            var decors = await _decorRepository.GetByIds(ids);

            if (decors.Length != ids.Length)
            {
                var missingIds = ids.Except(decors.Select(d => d.Id));
                var missingIdsString = string.Join(", ", missingIds);
                return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
            }

            await _decorRepository.UpdateCandleDecor(id, decors);

            return Result.Success();
        }

        public async Task<Result> UpdateLayerColor(int id, int[] ids)
        {
            var layerColors = await _layerColorRepository.GetByIds(ids);

            if (layerColors.Length != ids.Length)
            {
                var missingIds = ids.Except(layerColors.Select(l => l.Id));
                var missingIdsString = string.Join(", ", missingIds);
                return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
            }

            await _layerColorRepository.UpdateCandleLayerColor(id, layerColors);

            return Result.Success();
        }

        public async Task<Result> UpdateNumberOfLayer(int id, int[] ids)
        {
            var numberOfLayers = await _numberOfLayerRepository.GetByIds(ids);

            if (numberOfLayers.Length != ids.Length)
            {
                var missingIds = ids.Except(numberOfLayers.Select(l => l.Id));
                var missingIdsString = string.Join(", ", missingIds);
                return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
            }

            await _numberOfLayerRepository.UpdateCandleNumberOfLayer(id, numberOfLayers);

            return Result.Success();
        }

        public async Task<Result> UpdateSmell(int id, int[] ids)
        {
            var smells = await _smellRepository.GetByIds(ids);

            if (smells.Length != ids.Length)
            {
                var missingIds = ids.Except(smells.Select(l => l.Id));
                var missingIdsString = string.Join(", ", missingIds);
                return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
            }

            await _smellRepository.UpdateCandleSmell(id, smells);

            return Result.Success();
        }

        public async Task<Result> UpdateWick(int id, int[] ids)
        {
            var wicks = await _wickRepository.GetByIds(ids);

            if (wicks.Length != ids.Length)
            {
                var missingIds = ids.Except(wicks.Select(l => l.Id));
                var missingIdsString = string.Join(", ", missingIds);
                return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
            }

            await _wickRepository.UpdateCandleWick(id, wicks);

            return Result.Success();
        }
    }
}
