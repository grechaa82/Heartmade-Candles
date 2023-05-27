using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services
{
    public class CandleService : ICandleService
    {
        private readonly ICandleRepository _candleRepository;

        public CandleService(ICandleRepository candlerepository)
        {
            _candleRepository = candlerepository;
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
    }
}
