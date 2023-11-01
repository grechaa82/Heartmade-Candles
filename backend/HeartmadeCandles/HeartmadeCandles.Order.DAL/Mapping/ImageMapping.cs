using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class ImageMapping
{
    public static Image[] MapToImages(ImageCollection[] imagesCollection)
    {
        var images = new List<Image>();

        foreach (var imageCollection in imagesCollection)
        {
            var image = new Image(
                imageCollection.FileName,
                imageCollection.AlternativeName);

            images.Add(image);
        }

        return images.ToArray();
    }

    public static ImageCollection[] MapToImagesCollection(Image[] images)
    {
        var imageCollections = new List<ImageCollection>();

        foreach (var image in images)
        {
            var imageCollection = new ImageCollection
            {
                FileName = image.FileName,
                AlternativeName = image.AlternativeName
            };

            imageCollections.Add(imageCollection);
        }

        return imageCollections.ToArray();
    }
}
