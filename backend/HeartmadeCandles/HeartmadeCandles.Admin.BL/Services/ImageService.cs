using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using ImageMagick;

namespace HeartmadeCandles.Admin.BL.Services;

public class ImageService : IImageService
{
    private readonly string _staticFilesPath;
    private readonly Dictionary<int, string> _imageSizes = new Dictionary<int, string>()
    {
        { 1200, "large" },
        { 600, "medium" },
        { 200, "small" },
        { 20, "preview" },
    };

    public ImageService(string staticFilesPath)
    {
        _staticFilesPath = staticFilesPath;
    }

    public async Task<Result<string[]>> UploadImages(List<(Stream, string)> imageFiles)
    {
        var fileNames = new List<string>();

        foreach (var (originalStream, fileName) in imageFiles)
        {
            using (var clonedStream = new MemoryStream())
            {
                await originalStream.CopyToAsync(clonedStream);

                clonedStream.Position = 0;

                var imageResult = await SaveImage(clonedStream, fileName);

                if (imageResult.IsSuccess)
                {
                    fileNames.Add(imageResult.Value);
                    clonedStream.Position = 0;
                    await ResizeImage(clonedStream, imageResult.Value);
                }
            }
        }

        return Result.Success(fileNames.ToArray());
    }

    private async Task<Result<string>> SaveImage(Stream stream, string fileName)
    {
        using (MagickImage imageMagick = new MagickImage(stream))
        {
            var newFileName = GenerateNewFileName(fileName);

            var originalImagePath = Path.Combine(_staticFilesPath, newFileName);

            await Task.Run(() => imageMagick.Write(originalImagePath));

            stream.Position = 0;

            return Result.Success(newFileName);
        }
    }

    private string GenerateNewFileName(string fileName)
    {
        return Guid.NewGuid() + Path.GetExtension(fileName);
    }

    private async Task<Result> ResizeImage(Stream originalStream, string fileName)
    {
        var result = Result.Success();

        using (var image = new MagickImage(originalStream))
        {
            foreach (var (width, label) in _imageSizes)
            {
                result = Result.Combine(
                    result,
                    await ResizeAndSaveImage(image, fileName, width, label));

                result = Result.Combine(
                    result,
                    await ResizeAndSaveImage(image, fileName, width, label, MagickFormat.WebP));
            }
        }

        return result;
    }

    private async Task<Result> ResizeAndSaveImage(
        MagickImage imageMagick,
        string originalName,
        int width,
        string nameSuffix,
        MagickFormat? format = null)
    {
        try
        {
            MagickGeometry size = new MagickGeometry(width);
            imageMagick.Resize(size);

            var fileName = Path.GetFileNameWithoutExtension(originalName);
            var fileExtension = Path.GetExtension(originalName);
            var modifiedFileName = $"{fileName}-{nameSuffix}{(format.HasValue ? $".{format.Value.ToString().ToLower()}" : fileExtension)}";
            var outputImagePath = Path.Combine(_staticFilesPath, modifiedFileName ?? string.Empty);

            await Task.Run(() => imageMagick.Write(outputImagePath, format ?? MagickFormat.Unknown));

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure<string>($"Error saving image: {ex.Message}");
        }
    }
}
