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

        public async Task Update(CandleDetail candleDetail)
        {
            await _candleRepository.Update(candleDetail.Candle);
            await _decorRepository.UpdateCandleDecor(candleDetail.Candle.Id, candleDetail.Decors);
            await _layerColorRepository.UpdateCandleLayerColor(candleDetail.Candle.Id, candleDetail.LayerColors);
            await _numberOfLayerRepository.UpdateCandleNumberOfLayer(candleDetail.Candle.Id, candleDetail.NumberOfLayers);
            await _smellRepository.UpdateCandleSmell(candleDetail.Candle.Id, candleDetail.Smells);
            await _wickRepository.UpdateCandleWick(candleDetail.Candle.Id, candleDetail.Wicks);
        }

        public async Task Delete(int id)
        {
            await _candleRepository.Delete(id);
        }
    }
}
