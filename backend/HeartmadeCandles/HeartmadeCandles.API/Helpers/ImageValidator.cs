using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;

namespace HeartmadeCandles.API.Helpers
{
    public class ImageValidator
    {
        public static Result<Image[]> ValidateImages(ImageRequest[] imageRequests)
        {
            var imagesResult = imageRequests
                .Select(imageRequest => Image.Create(imageRequest.FileName, imageRequest.AlternativeName))
                .ToArray();

            if (imagesResult.Any(result => result.IsFailure))
            {
                var failedImagesResult = imagesResult.Where(result => result.IsFailure).ToArray();
                var errorMessages = string.Join(", ", failedImagesResult.Select(result => result.Error));
                return Result.Failure<Image[]>(errorMessages);
            }

            var images = imagesResult
                .Select(result => result.Value)
                .ToArray();

            return Result.Success(images);
        }
    }
}
