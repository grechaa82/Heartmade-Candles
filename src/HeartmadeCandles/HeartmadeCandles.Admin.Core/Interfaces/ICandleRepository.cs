using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ICandleRepository
    {
        Task<Candle[]> GetAll();

        Task<CandleDetail> Get(int id);

        Task Create(Candle candle);

        Task Update(Candle candle);

        Task Delete(int id);
    }
}
