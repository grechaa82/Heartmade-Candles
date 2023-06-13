using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ILayerColorService
    {
        Task<LayerColor[]> GetAll();
        Task<LayerColor> Get(int id);
        Task Create(LayerColor layerColor);
        Task Update(LayerColor layerColor);
        Task Delete(int id);
    }
}
