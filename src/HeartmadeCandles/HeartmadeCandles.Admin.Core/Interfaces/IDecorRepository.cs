using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface IDecorRepository
    {
        Task<Decor[]> GetAll();
        Task<Decor> Get(int id);
        Task<Decor[]> GetByIds(int[] ids);
        Task Create(Decor decor);
        Task Update(Decor decor);
        Task Delete(int id);
        Task UpdateCandleDecor(int candleId, Decor[] decors);
        Task<bool> AreIdsExist(int[] ids);
        Task<int[]> GetNonExistingIds(int[] ids);
    }
}
