using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Repositories
{
    public interface ICandleConstructorRepository
    {
        Task CreateOrder(Order order);
        Task<List<Candle>> GetAllAsync();
    }
}