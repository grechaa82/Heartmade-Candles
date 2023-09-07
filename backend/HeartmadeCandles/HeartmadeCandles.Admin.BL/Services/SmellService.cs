using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services
{
    public class SmellService : ISmellService
    {
        private readonly ISmellRepository _smellRepository;

        public SmellService(ISmellRepository smellRepository)
        {
            _smellRepository = smellRepository;
        }

        public async Task<Smell[]> GetAll()
        {
            return await _smellRepository.GetAll();
        }
        
        public async Task<Smell> Get(int smellId)
        {
            return await _smellRepository.Get(smellId);
        }
        
        public async Task Create(Smell smell)
        {
            await _smellRepository.Create(smell);
        }
        
        public async Task Update(Smell smell)
        {
            await _smellRepository.Update(smell);
        }

        public async Task Delete(int smellId)
        {
            await _smellRepository.Delete(smellId);
        }
    }
}
