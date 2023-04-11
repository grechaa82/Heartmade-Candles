using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Repositories
{
    public interface IShoppingCartRepository
    {
        Task CreateOrderAsync(Order order);
        Task<List<ShoppingCartItem>> GetByUserIdAsync(string userId);
    }
}
