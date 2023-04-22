using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services
{
    public class DecorService : IDecorService
    {
        private readonly IDecorRepository _decorRepository;

        public DecorService(IDecorRepository decorRepository)
        {
            _decorRepository = decorRepository;
        }

        public async Task<List<Decor>> GetAll()
        {
            return await _decorRepository.GetAll();
        }

        public async Task<Decor> Get(int id)
        {
            return await _decorRepository.Get(id);
        }

        public async Task Create(Decor decor)
        {
            await _decorRepository.Create(decor);
        }

        public async Task Update(Decor decor)
        {
            await _decorRepository.Update(decor);
        }

        public async Task Delete(int id)
        {
            await _decorRepository.Delete(id);
        }
    }
}
