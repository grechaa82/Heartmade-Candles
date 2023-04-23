using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface IWickRepository
    {
        Task<List<Wick>> GetAll();
        Task<Wick> Get(int id);
        Task Create(Wick wick);
        Task Update(Wick wick);
        Task Delete(int id);
    }
}
