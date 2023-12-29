using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class SmellMapping
{
    public static Smell MapToSmell(SmellDocument smellDocument)
    {
        return new Smell
        {
            Id = smellDocument.Id,
            Title = smellDocument.Title,
            Price = smellDocument.Price
        };
    }

    public static SmellDocument MapToSmellDocument(Smell smell)
    {
        return new SmellDocument
        {
            Id = smell.Id,
            Title = smell.Title,
            Price = smell.Price
        };
    }
}

