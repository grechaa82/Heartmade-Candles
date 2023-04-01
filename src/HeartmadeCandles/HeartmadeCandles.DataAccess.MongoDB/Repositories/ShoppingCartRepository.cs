using AutoMapper;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.DataAccess.MongoDB.Collections;
using MongoDB.Driver;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IMongoCollection<ShoppingCartItemCollection> _shoppingCartItem;
        private readonly IMapper _mapper;

        public ShoppingCartRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _shoppingCartItem = mongoDatabase.GetCollection<ShoppingCartItemCollection>("ShoppingCartItem");
            _mapper = mapper;
        }

        public async Task<List<ShoppingCartItem>> GetByUserIdAsync(string userId)
        {
            var shoppingCartItems = await _shoppingCartItem.Find(_ => _.UserId == userId).ToListAsync();

            return _mapper.Map<List<ShoppingCartItemCollection>, List<ShoppingCartItem>>(shoppingCartItems);
        }
    }
}
