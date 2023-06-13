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

        public async Task<IList<Candle>> GetAll()
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
            if (!await _decorRepository.AreIdsExist(ids))
            {
                var nonExistingIds = await _decorRepository.GetNonExistingIds(ids);
                var nonExistingIdsString = string.Join(", ", nonExistingIds);
                return Result.Failure<int[]>($"'{nonExistingIdsString}' these ids do not exist");
            }

            var decors = await _decorRepository.GetByIds(ids);

            await _decorRepository.UpdateCandleDecor(id, decors.ToList());

            return Result.Success();
        }

        public async Task<Result> UpdateLayerColor(int id, int[] ids)
        {
            try
            {
                if (!await _layerColorRepository.AreIdsExist(ids))
                {
                    var nonExistingIds = await _layerColorRepository.GetNonExistingIds(ids);
                    var nonExistingIdsString = string.Join(", ", nonExistingIds);
                    return Result.Failure<int[]>($"'{nonExistingIdsString}' these ids do not exist");
                }

                var layerColors = await _layerColorRepository.GetByIds(ids);

                await _layerColorRepository.UpdateCandleLayerColor(id, layerColors.ToList());

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<bool> UpdateNumberOfLayer(int id, int[] ids)
        {
            if (!await _layerColorRepository.AreIdsExist(ids))
            {
                return false;
            }

            var layerColors = await _layerColorRepository.GetByIds(ids);

            await _layerColorRepository.UpdateCandleLayerColor(id, layerColors.ToList());

            return true;
        }

        public async Task<Result> UpdateSmell(int id, int[] ids)
        {
            if (!await _smellRepository.AreIdsExist(ids))
            {
                var nonExistingIds = await _smellRepository.GetNonExistingIds(ids);
                var nonExistingIdsString = string.Join(", ", nonExistingIds);
                return Result.Failure<int[]>($"'{nonExistingIdsString}' these ids do not exist");
            }

            var smells = await _smellRepository.GetByIds(ids);

            await _smellRepository.UpdateCandleSmell(id, smells.ToList());

            return Result.Success();
        }

        public async Task<Result> UpdateWick(int id, int[] ids)
        {
            if (!await _wickRepository.AreIdsExist(ids))
            {
                var nonExistingIds = await _wickRepository.GetNonExistingIds(ids);
                var nonExistingIdsString = string.Join(", ", nonExistingIds);
                return Result.Failure<int[]>($"'{nonExistingIdsString}' these ids do not exist");
            }

            var wicks = await _wickRepository.GetByIds(ids);

            await _wickRepository.UpdateCandleWick(id, wicks.ToList());

            return Result.Success();
        }
    }
}
