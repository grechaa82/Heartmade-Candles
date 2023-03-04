using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface ICandleConstructorService
    {
        Task CreateOrder(Order order);
        Task<List<Candle>> GetAllAsync();
    }
}
