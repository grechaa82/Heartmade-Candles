using HeartmadeCandles.Core;
using HeartmadeCandles.Core.Interfaces;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.BusinessLogic.Services
{
    
    public class CandleConstructorService : ICandleConstructorService
    {
        public delegate void OrderEventHandler (Order order);
        public event OrderEventHandler OrderCreated;

        //private readonly IOrderCreateHandler _orderCreateHandler;

        private readonly ICandleConstructorRepository _candleConstructorRepository;
        private readonly IUserRepository _userRepository;

        public CandleConstructorService()
        {
        }

        public CandleConstructorService(
            ICandleConstructorRepository candleConstructorRepository, 
            //IOrderCreateHandler orderCreateHandler, 
            IUserRepository userRepository)
        {
            _candleConstructorRepository = candleConstructorRepository;
            //_orderCreateHandler = orderCreateHandler;
            _userRepository = userRepository;
        }

        public async Task<List<CandleMinimal>> GetAllAsync()
        {
            var candles = await _candleConstructorRepository.GetAllAsync();
            var candleMinimal = new List<CandleMinimal>();

            foreach (var candle in candles)
            {
                var result = new CandleMinimal(
                    candle.Title,
                    candle.ImageURL,
                    candle.TypeCandle,
                    candle.Id,
                    candle.Description);

                candleMinimal.Add(result);
            }

            return candleMinimal;
        }

        public async Task<Candle> GetByIdAsync(string id)
        {
            var candle = await _candleConstructorRepository.GetByIdAsync(id);

            if (candle == default)
            {
                throw new ArgumentNullException($"'{nameof(candle.Id)}' could not find.)");
            }

            return candle;
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

            var order = new Order(
                user,
                customer,
                candles,
                shoppingCart.Description);

            await _candleConstructorRepository.CreateOrder(order);
            //_orderCreateHandler.OnOrderCreated(order);

            return true;
        }
    }
}
