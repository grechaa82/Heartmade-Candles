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

        public async Task<ShoppingCart> Get(string id)
        {
            List<ShoppingCartItem> shoppingCartItems = await _shoppingCartRepository.Get(id);

            var shoppingCart = ShoppingCart.Create(id, shoppingCartItems);

            throw new NotImplementedException();
        }
    }
}
