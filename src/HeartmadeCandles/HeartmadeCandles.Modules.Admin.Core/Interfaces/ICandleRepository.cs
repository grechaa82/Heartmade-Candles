using HeartmadeCandles.Modules.Admin.Core.Models;

namespace HeartmadeCandles.Modules.Admin.Core.Interfaces
{
    public interface ICandleRepository
    {
        Task<IList<Candle>> GetAll();

        Task<IList<Candle>> Get(int id);

        Task Create(Candle candle);

        Task Update(Candle candle);

        Task Delete(int id);
    }
}
