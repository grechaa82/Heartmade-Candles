using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface IDecorRepository
{
    Task<Decor[]> GetAll();
    Task<Decor> Get(int decorId);
    Task<Decor[]> GetByIds(int[] decorIds);
    Task Create(Decor decor);
    Task Update(Decor decor);
    Task Delete(int decorId);
    Task UpdateCandleDecor(int candleId, Decor[] decors);
}