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

        public async Task Create(
            int id, 
            string title, 
            string description, 
            string imageURL, 
            int weightGrams, 
            bool isUsed, 
            TypeCandle typeCandle)
        {
            var candle = new Candle(
                id, 
                title, 
                description, 
                imageURL, 
                weightGrams, 
                isUsed, 
                typeCandle);
        }

        public async Task<IList<Candle>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Candle> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Candle candle)
        {
            throw new NotImplementedException();
        }
    }
}
