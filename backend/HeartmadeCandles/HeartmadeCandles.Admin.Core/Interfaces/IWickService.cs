using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface IWickService
    {
        Task<Wick[]> GetAll();
        Task<Wick> Get(int wickId);
        Task Create(Wick wick);
        Task Update(Wick wick);
        Task Delete(int wickId);
    }
}
