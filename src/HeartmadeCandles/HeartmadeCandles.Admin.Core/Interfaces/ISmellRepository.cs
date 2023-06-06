using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ISmellRepository
    {
        Task<List<Smell>> GetAll();
        Task<Smell> Get(int id);
        Task Create(Smell smell);
        Task Update(Smell smell);
        Task Delete(int id);
        Task UpdateCandleSmell(int candleId, List<Smell> smells);
    }
}
