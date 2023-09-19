using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface IWickRepository
{
    Task<Wick[]> GetAll();
    Task<Wick> Get(int wickId);
    Task<Wick[]> GetByIds(int[] wickIds);
    Task Create(Wick wick);
    Task Update(Wick wick);
    Task Delete(int wickId);
    Task UpdateCandleWick(int candleId, Wick[] wicks);
}