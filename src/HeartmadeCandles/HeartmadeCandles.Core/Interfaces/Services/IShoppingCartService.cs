using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> Get(string id);
    }
}
