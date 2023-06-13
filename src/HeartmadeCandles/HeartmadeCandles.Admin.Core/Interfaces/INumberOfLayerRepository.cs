using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface INumberOfLayerRepository
    {
        Task<List<NumberOfLayer>> GetAll();
        Task<NumberOfLayer> Get(int id);
        Task<NumberOfLayer[]> GetByIds(int[] ids);
        Task Create(NumberOfLayer numberOfLayer);
        Task Update(NumberOfLayer numberOfLayer);
        Task Delete(int id);
        Task UpdateCandleNumberOfLayer(int candleId, List<NumberOfLayer> numberOfLayers);
        Task<bool> AreIdsExist(int[] ids);
        Task<int[]> GetNonExistingIds(int[] ids);

    }
}
