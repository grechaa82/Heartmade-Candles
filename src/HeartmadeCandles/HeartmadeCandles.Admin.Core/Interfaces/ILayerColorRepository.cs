using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ILayerColorRepository
    {
        Task<LayerColor[]> GetAll();
        Task<LayerColor> Get(int layerColorId);
        Task<LayerColor[]> GetByIds(int[] layerColorIds);
        Task Create(LayerColor layerColor);
        Task Update(LayerColor layerColor);
        Task Delete(int layerColorId);
        Task UpdateCandleLayerColor(int candleId, LayerColor[] layerColors);
    }
}
