using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface IDecorRepository
    {
        Task<List<Decor>> GetAll();
        Task<Decor> Get(int id);
        Task Create(Decor decor);
        Task Update(Decor decor);
        Task Delete(int id);
        Task UpdateCandleDecor(int candleId, List<Decor> decors);
    }
}
