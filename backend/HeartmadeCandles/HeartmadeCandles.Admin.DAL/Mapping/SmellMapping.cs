using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;

namespace HeartmadeCandles.Admin.DAL.Mapping;

internal class SmellMapping
{
    public static Smell MapToSmell(SmellEntity smellEntity)
    {
        var smell = Smell.Create(
            smellEntity.Title,
            smellEntity.Description,
            smellEntity.Price,
            smellEntity.IsActive,
            smellEntity.Id);

        return smell.Value;
    }

    public static SmellEntity MapToSmellEntity(Smell smell)
    {
        var smellEntity = new SmellEntity
        {
            Id = smell.Id,
            Title = smell.Title,
            Description = smell.Description,
            Price = smell.Price,
            IsActive = smell.IsActive
        };

        return smellEntity;
    }
}