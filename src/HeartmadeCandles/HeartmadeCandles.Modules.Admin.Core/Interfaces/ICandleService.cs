using HeartmadeCandles.Modules.Admin.Core.Models;

namespace HeartmadeCandles.Modules.Admin.Core.Interfaces
{
    public interface ICandleService
    {
        Task Create(
            int id,
            string title,
            string description,
            string imageURL,
            int weightGrams,
            bool isUsed,
            TypeCandle typeCandle);

        Task<IList<Candle>> GetAll();

        Task<Candle> Get(int id);

        Task Update(Candle candle);

        Task Delete(int id);
    }
}
