using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ILayerColorRepository
    {
        Task<List<LayerColor>> GetAll();
        Task<LayerColor> Get(int id);
        Task<LayerColor[]> GetByIds(int[] ids);
        Task Create(LayerColor layerColor);
        Task Update(LayerColor layerColor);
        Task Delete(int id);
        Task UpdateCandleLayerColor(int candleId, List<LayerColor> layerColors);
        Task<bool> AreIdsExist(int[] ids);
        Task<int[]> GetNonExistingIds(int[] ids);
    }
}
