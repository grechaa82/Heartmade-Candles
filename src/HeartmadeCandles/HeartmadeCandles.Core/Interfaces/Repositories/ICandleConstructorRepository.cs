using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Repositories
{
    public interface ICandleConstructorRepository
    {
        Task<List<Candle>> GetAllAsync();
        Task<Candle> GetByIdAsync(string id);
        Task CreateOrder(Order order);
    }
}
