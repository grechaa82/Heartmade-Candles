using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class WickMapping
{
    public static Wick MapToWick(WickDocument wickDocument)
    {
        return new Wick
        {
            Id = wickDocument.Id,
            Title = wickDocument.Title,
            Price = wickDocument.Price
        };
    }

    public static WickDocument MapToWickDocument(Wick wick)
    {
        return new WickDocument
        {
            Id = wick.Id,
            Title = wick.Title,
            Price = wick.Price
        };
    }
}
