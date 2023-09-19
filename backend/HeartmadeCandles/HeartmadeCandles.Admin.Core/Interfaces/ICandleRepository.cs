using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ICandleRepository
{
    Task<Candle[]> GetAll();

    Task<Candle> GetById(int candleId);
    Task<CandleDetail> GetCandleDetailById(int candleId);

    Task Create(Candle candle);

    Task Update(Candle candle);

    Task Delete(int candleId);
}