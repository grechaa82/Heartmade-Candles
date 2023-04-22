using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services
{
    public class LayerColorService : ILayerColorService
    {
        private readonly ILayerColorRepository _layerColorRepository;

        public LayerColorService(ILayerColorRepository layerColorRepository)
        {
            _layerColorRepository = layerColorRepository;
        }

        public async Task<List<LayerColor>> GetAll()
        {
            return await _layerColorRepository.GetAll();
        }

        public async Task<LayerColor> Get(int id)
        {
            return await _layerColorRepository.Get(id);
        }

        public async Task Create(LayerColor layerColor)
        {
            await _layerColorRepository.Create(layerColor);
        }

        public async Task Update(LayerColor layerColor)
        {
            await _layerColorRepository.Update(layerColor);
        }

        public async Task Delete(int id)
        {
            await _layerColorRepository.Delete(id);
        }
    }
}
