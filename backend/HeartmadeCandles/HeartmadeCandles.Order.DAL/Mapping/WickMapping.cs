using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class WickMapping
{
    public static Wick MapToWick(WickCollection wickCollection)
    {
        return new Wick(
            wickCollection.Id,
            wickCollection.Title,
            wickCollection.Price);
    }

    public static WickCollection MapToWickCollection(Wick wick)
    {
        return new WickCollection
        {
            Id = wick.Id,
            Title = wick.Title,
            Price = wick.Price
        };
    }
}
