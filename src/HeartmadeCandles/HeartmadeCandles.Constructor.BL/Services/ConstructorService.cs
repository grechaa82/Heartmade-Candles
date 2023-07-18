using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Constructor.Core.Models;

namespace HeartmadeCandles.Constructor.BL.Services
{
    public class ConstructorService : IConstructorService
    {
        private readonly IConstructorRepository _constructorRepository;

        public ConstructorService(IConstructorRepository constructorRepository)
        {
            _constructorRepository = constructorRepository;
        }

        public async Task<CandleTypeWithCandles[]> GetAll()
        {
            return await _constructorRepository.GetAll();
        }
    }
}
