using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class ImageMapping
{
    public static Image[] MapToImages(ImageDocument[] imagesDocuments)
    {
        var images = new List<Image>();

        foreach (var imageDocument in imagesDocuments)
        {
            var image = new Image(
                imageDocument.FileName,
                imageDocument.AlternativeName);

            images.Add(image);
        }

        return images.ToArray();
    }

    public static ImageDocument[] MapToImagesDocument(Image[] images)
    {
        var imageDocuments = new List<ImageDocument>();

        foreach (var image in images)
        {
            var imageDocument = new ImageDocument
            {
                FileName = image.FileName,
                AlternativeName = image.AlternativeName
            };

            imageDocuments.Add(imageDocument);
        }

        return imageDocuments.ToArray();
    }
}
