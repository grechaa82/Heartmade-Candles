using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ICandleService
    {
        Task Create(Candle candle);

        Task<IList<Candle>> GetAll();

        Task<CandleDetail> Get(int id);

        Task Update(Candle candle);

        Task Delete(int id);
    }
}
