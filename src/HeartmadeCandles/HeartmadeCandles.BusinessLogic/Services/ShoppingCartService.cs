using HeartmadeCandles.Core;
using HeartmadeCandles.Core.Interfaces;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.BusinessLogic.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderCreateHandler _orderCreateHandler;

        public ShoppingCartService(
            IShoppingCartRepository shoppingCartRepository, 
            IUserRepository userRepository, 
            IOrderCreateHandler orderCreateHandler)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderCreateHandler = orderCreateHandler;
        }

        public async Task<ShoppingCart> Get(string userId)
        {
            var shoppingCartItems = await _shoppingCartRepository.GetByUserIdAsync(userId);

            var shoppingCart = new ShoppingCart(userId, shoppingCartItems);

            return shoppingCart;
        }

        public async Task<bool> CreateOrder(ShoppingCart shoppingCart)
        {
            var user = await _userRepository.GetUserAsync(shoppingCart.UserId);
            var customer = await _userRepository.GetCustomerAsync(user.CustomerId);
            var candles = new Dictionary<int, Candle>();

            foreach (var item in shoppingCart.Items)
            {
                candles.Add(item.Quantity, item.Candle);
            };

            if (user == null || customer == null || !candles.Any())
            {
                throw new ArgumentNullException($"'{nameof(user)}' or '{nameof(customer)}' or '{nameof(candles)}' connot be null.");
            }

            var order = new Order(
                user: user, 
                customer: customer, 
                candles: candles,
                description: shoppingCart.Description);

            await _shoppingCartRepository.CreateOrderAsync(order);
            _orderCreateHandler.OnOrderCreated(order);

            return true;
        }
    }
}
