using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ITypeCandleService
{
    Task<TypeCandle[]> GetAll();
    Task<TypeCandle> Get(int typeCandleId);
    Task Create(TypeCandle typeCandle);
    Task Update(TypeCandle typeCandle);
    Task Delete(int typeCandleId);
}