using HeartmadeCandles.Modules.Admin.Core.Interfaces;
using HeartmadeCandles.Modules.Admin.Core.Models;

namespace HeartmadeCandles.Modules.Admin.BL.Services
{
    public class CandleService : ICandleService
    {
        private readonly ICandleRepository _candlerepository;

        public CandleService(ICandleRepository candlerepository)
        {
            _candlerepository = candlerepository;
        }

        public async Task<IList<Candle>> GetAll()
        {
            return await _candlerepository.GetAll();
        }

        public async Task<Candle> Get(int id)
        {
            return await _candlerepository.Get(id);
        }

        public async Task Create(Candle candle)
        {
            _candlerepository.Create(candle);
        }

        public async Task Update(Candle candle)
        {
            _candlerepository.Update(candle);
        }

        public async Task Delete(int id)
        {
            await _candlerepository.Delete(id);
        }
    }
}
