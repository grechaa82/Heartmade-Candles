using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;

namespace HeartmadeCandles.Admin.DAL.Mapping;

internal class DecorMapping
{
    public static Decor MapToDecor(DecorEntity decorEntity)
    {
        var decor = Decor.Create(
            decorEntity.Title,
            decorEntity.Description,
            decorEntity.Price,
            ImageMapping.MapToImage(decorEntity.Images),
            decorEntity.IsActive,
            decorEntity.Id);

        return decor.Value;
    }

    public static DecorEntity MapToDecorEntity(Decor decor)
    {
        var decorEntity = new DecorEntity
        {
            Id = decor.Id,
            Title = decor.Title,
            Description = decor.Description,
            Price = decor.Price,
            Images = ImageMapping.MapToImageEntity(decor.Images),
            IsActive = decor.IsActive
        };

        return decorEntity;
    }
}