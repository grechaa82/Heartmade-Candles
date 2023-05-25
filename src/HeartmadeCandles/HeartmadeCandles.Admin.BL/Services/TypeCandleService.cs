using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services
{
    public class TypeCandleService : ITypeCandleService
    {
        private readonly ITypeCandleRepository _typeCandleRepository;

        public TypeCandleService(ITypeCandleRepository typeCandleRepository)
        {
            _typeCandleRepository = typeCandleRepository;
        }

        public async Task<List<TypeCandle>> GetAll()
        {
            return await _typeCandleRepository.GetAll();
        }

        public async Task<TypeCandle> Get(int id)
        {
            return await _typeCandleRepository.Get(id);
        }

        public async Task Create(TypeCandle typeCandle)
        {
            await _typeCandleRepository.Create(typeCandle);
        }

        public async Task Update(TypeCandle typeCandle)
        {
            await _typeCandleRepository.Update(typeCandle);
        }

        public async Task Delete(int id)
        {
            await _typeCandleRepository.Delete(id);
        }
    }
}
