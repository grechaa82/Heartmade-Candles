using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface IShoppingCartService
    {
        Task<bool> CreateOrder(ShoppingCart shoppingCart);
        Task<ShoppingCart> Get(string userId);
    }
}
