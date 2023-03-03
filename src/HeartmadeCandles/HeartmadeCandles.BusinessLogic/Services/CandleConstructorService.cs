using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.BusinessLogic.Services
{
    public class CandleConstructorService : ICandleConstructorService
    {
        private readonly ICandleConstructorRepository _candleConstructorRepository;
        public CandleConstructorService(ICandleConstructorRepository candleConstructorRepository)
        {
            _candleConstructorRepository = candleConstructorRepository;
        }

        public async Task<List<Candle>> GetAllAsync()
        {
            return await _candleConstructorRepository.GetAllAsync();
        }
    }
}
