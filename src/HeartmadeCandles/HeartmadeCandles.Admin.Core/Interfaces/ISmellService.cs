using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ISmellService
    {
        Task<List<Smell>> GetAll();
        Task<Smell> Get(int id);
        Task Create(Smell smell);
        Task Update(Smell smell);
        Task Delete(int id);
    }
}
