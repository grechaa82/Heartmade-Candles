using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface INumberOfLayerService
    {
        Task<NumberOfLayer[]> GetAll();
        Task<NumberOfLayer> Get(int id);
        Task Create(NumberOfLayer numberOfLayer);
        Task Update(NumberOfLayer numberOfLayer);
        Task Delete(int id);
    }
}
