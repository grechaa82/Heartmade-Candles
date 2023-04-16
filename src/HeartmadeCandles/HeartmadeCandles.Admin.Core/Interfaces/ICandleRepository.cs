using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ICandleRepository
    {
        Task<IList<Candle>> GetAll();

        Task<Candle> Get(int id);

        Task Create(Candle candle);

        Task Update(Candle candle);

        Task Delete(int id);
    }
}
