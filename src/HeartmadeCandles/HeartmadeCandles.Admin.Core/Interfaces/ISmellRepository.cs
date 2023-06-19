using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ISmellRepository
    {
        Task<Smell[]> GetAll();
        Task<Smell> Get(int id);
        Task<Smell[]> GetByIds(int[] ids);
        Task Create(Smell smell);
        Task Update(Smell smell);
        Task Delete(int id);
        Task UpdateCandleSmell(int candleId, Smell[] smells);
    }
}
