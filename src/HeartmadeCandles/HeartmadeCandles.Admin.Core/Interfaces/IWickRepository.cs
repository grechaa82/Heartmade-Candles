using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface IWickRepository
    {
        Task<List<Wick>> GetAll();
        Task<Wick> Get(int id);
        Task<Wick[]> GetByIds(int[] ids);
        Task Create(Wick wick);
        Task Update(Wick wick);
        Task Delete(int id);
        Task UpdateCandleWick(int candleId, List<Wick> wicks);
        Task<bool> AreIdsExist(int[] ids);
        Task<int[]> GetNonExistingIds(int[] ids);
    }
}
