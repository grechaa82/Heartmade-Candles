using AutoMapper;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.DataAccess.MongoDB.Collections;
using MongoDB.Driver;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IMongoCollection<OrderCollection> _orderCollection;
        private readonly IMongoCollection<ShoppingCartItemCollection> _shoppingCartItem;
        private readonly IMapper _mapper;

        public ShoppingCartRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _orderCollection = mongoDatabase.GetCollection<OrderCollection>("Order");
            _shoppingCartItem = mongoDatabase.GetCollection<ShoppingCartItemCollection>("ShoppingCartItem");
            _mapper = mapper;
        }

        public async Task<List<ShoppingCartItem>> GetByUserIdAsync(string userId)
        {
            var shoppingCartItems = await _shoppingCartItem.Find(_ => _.UserId == userId).ToListAsync();

            return _mapper.Map<List<ShoppingCartItemCollection>, List<ShoppingCartItem>>(shoppingCartItems);
        }

        public async Task CreateOrderAsync(Order order)
        {
            var orderCollection = _mapper.Map<Order, OrderCollection>(order);
            await _orderCollection.InsertOneAsync(orderCollection);
        }
    }
}
