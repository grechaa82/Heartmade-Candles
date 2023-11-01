using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class SmellMapping
{
    public static Smell MapToSmell(SmellCollection smellCollection)
    {
        return new Smell(
            smellCollection.Id,
            smellCollection.Title,
            smellCollection.Price
        );
    }

    public static SmellCollection MapToSmellCollection(Smell smell)
    {
        return new SmellCollection
        {
            Id = smell.Id,
            Title = smell.Title,
            Price = smell.Price
        };
    }
}

