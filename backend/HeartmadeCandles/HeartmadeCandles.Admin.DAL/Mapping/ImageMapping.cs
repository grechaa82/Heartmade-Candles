using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;

namespace HeartmadeCandles.Admin.DAL.Mapping;

internal class ImageMapping
{
    public static ImageEntity[] MapToImageEntity(Image[] images)
    {
        var imageEntities = new List<ImageEntity>();

        foreach (var image in images)
        {
            var imageEntity = new ImageEntity
            {
                FileName = image.FileName,
                AlternativeName = image.AlternativeName
            };

            imageEntities.Add(imageEntity);
        }

        return imageEntities.ToArray();
    }

    public static Image[] MapToImage(ImageEntity[] imageEntities)
    {
        var images = new List<Image>();

        foreach (var imageEntity in imageEntities)
        {
            var image = Image.Create(imageEntity.FileName, imageEntity.AlternativeName);

            images.Add(image.Value);
        }

        return images.ToArray();
    }
}