using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class DecorMapping
{
    public static Decor MapToDecor(DecorDocument decorDocument)
    {
        return new Decor(
            decorDocument.Id,
            decorDocument.Title,
            decorDocument.Price
        );
    }

    public static DecorDocument MapToDecorDocument(Decor decor)
    {
        return new DecorDocument
        {
            Id = decor.Id,
            Title = decor.Title,
            Price = decor.Price
        };
    }
}

