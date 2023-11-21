using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using MongoDB.Driver;
using HeartmadeCandles.Order.DAL.Documents;
using HeartmadeCandles.Order.DAL.Mapping;


namespace HeartmadeCandles.Order.DAL;

public class BasketRepository : IBasketRepository
{
    private readonly IMongoCollection<BasketDocument> _basketCollection;

    public BasketRepository(IMongoDatabase mongoDatabase)
    {
        _basketCollection = mongoDatabase.GetCollection<BasketDocument>("basket");
    }

    public async Task<Maybe<Basket>> GetBasketById(string basketId)
    {
        var basketDocument = await _basketCollection
            .Find(x => x.Id == basketId)
            .FirstOrDefaultAsync();

        if (basketDocument == null)
        {
            return Maybe<Basket>.None;
        }

        var basket = BasketMapping.MapToBasket(basketDocument);

        return basket;
    }

    public async Task<Result<string>> CreateBasket(Basket basket)
    {
        var basketDocument = new BasketDocument
        {
            Items = BasketItemMapping.MapToBasketItemDocument(basket.Items),
            TotalPrice = basket.TotalPrice,
            TotalQuantity = basket.TotalQuantity,
            FilterString = basket.FilterString,
        };

        await _basketCollection.InsertOneAsync(basketDocument);

        return Result.Success(basketDocument.Id);
    }
}
