using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.BusinessLogic.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<ShoppingCart> Get(string userId)
        {
            var shoppingCartItems = await _shoppingCartRepository.GetByUserIdAsync(userId);

            var shoppingCart = ShoppingCart.Create(userId, shoppingCartItems);

            return shoppingCart;
        }
    }
}
