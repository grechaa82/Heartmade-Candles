using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface INumberOfLayerRepository
    {
        Task<NumberOfLayer[]> GetAll();
        Task<NumberOfLayer> Get(int numberOfLayerId);
        Task<NumberOfLayer[]> GetByIds(int[] numberOfLayerIds);
        Task Create(NumberOfLayer numberOfLayer);
        Task Update(NumberOfLayer numberOfLayer);
        Task Delete(int numberOfLayerId);
        Task UpdateCandleNumberOfLayer(int candleId, NumberOfLayer[] numberOfLayers);
    }
}
