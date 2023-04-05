using HeartmadeCandles.Core.Interfaces;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.BusinessLogic.Services
{

    public class CandleConstructorService : ICandleConstructorService
    {
        public delegate void OrderEventHandler(Order order);
        public event OrderEventHandler OrderCreated;

        private readonly IOrderCreateHandler _orderCreateHandler;

        private readonly ICandleConstructorRepository _candleConstructorRepository;

        public CandleConstructorService(ICandleConstructorRepository candleConstructorRepository, IOrderCreateHandler orderCreateHandler)
        {
            _candleConstructorRepository = candleConstructorRepository;
            _orderCreateHandler = orderCreateHandler;
        }

        public CandleConstructorService()
        {
        }

        public async Task<List<Candle>> GetAllAsync()
        {
            return await _candleConstructorRepository.GetAllAsync();
        }

        public async Task CreateOrder(Order order)
        {
            await _candleConstructorRepository.CreateOrder(order);

            _orderCreateHandler.OnOrderCreated(order);
        }
    }
}
