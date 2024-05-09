using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface IImageService
{
    Task<Result<string[]>> UploadImages(List<(Stream, string)> imageFiles);
}
