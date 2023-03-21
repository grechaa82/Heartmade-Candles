using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using MongoDB.Driver;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IMongoCollection<ShoppingCartItem> _shoppingCartItem;

        public ShoppingCartRepository(IMongoDatabase mongoDatabase)
        {
            _shoppingCartItem = mongoDatabase.GetCollection<ShoppingCartItem>("ShoppingCartItem");
        }

        public Task<List<ShoppingCartItem>> Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}
