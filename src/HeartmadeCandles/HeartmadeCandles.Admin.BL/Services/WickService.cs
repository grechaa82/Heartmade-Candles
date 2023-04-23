using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services
{
    public class WickService : IWickService
    {
        private readonly IWickRepository _wickRepository;

        public WickService(IWickRepository wickRepository)
        {
            _wickRepository = wickRepository;
        }

        public async Task<List<Wick>> GetAll()
        {
            return await _wickRepository.GetAll();
        }

        public async Task<Wick> Get(int id)
        {
            return await _wickRepository.Get(id);
        }

        public async Task Create(Wick wick)
        {
            await _wickRepository.Create(wick);
        }

        public async Task Update(Wick wick)
        {
            await _wickRepository.Update(wick);
        }

        public async Task Delete(int id)
        {
            await _wickRepository.Delete(id);
        }
    }
}
