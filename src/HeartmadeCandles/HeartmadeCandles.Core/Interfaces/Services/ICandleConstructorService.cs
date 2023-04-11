using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface ICandleConstructorService
    {
        Task<List<CandleMinimal>> GetAllAsync();
        Task<Candle> GetByIdAsync(string id);
        Task<bool> CreateOrder(ShoppingCart shoppingCart);
    }
}
