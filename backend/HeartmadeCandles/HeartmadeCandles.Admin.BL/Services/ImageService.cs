using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using ImageMagick;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<ImageService> _logger;

    public ImageService(string staticFilesPath, ILogger<ImageService> logger)
    {
        _staticFilesPath = staticFilesPath;
        _logger = logger;
    }

    public async Task<Result<string[]>> UploadImages(List<(Stream, string)> imageFiles)
    {
        var fileNames = new List<Result<string>>();

        foreach (var (originalStream, fileName) in imageFiles)
        {
            using var clonedStream = new MemoryStream();

            var newFileName = await CloneStream(originalStream, clonedStream)
                .Bind(() => SaveImage(clonedStream, fileName))
                .Bind(newFileName => ResizeImage(clonedStream, newFileName).Map(() => newFileName));

            fileNames.Add(newFileName);
        }

        return Result.Combine(fileNames).Map(() => fileNames.Select(fn => fn.Value).ToArray());
    }

    private async Task<Result> CloneStream(Stream sourceStream, Stream destinationStream)
    {
        try
        {
            await sourceStream.CopyToAsync(destinationStream);
            destinationStream.Position = 0;
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error copying stream: {ex.Message}");
        }
    }

    private async Task<Result<string>> SaveImage(Stream stream, string fileName)
    {
        using (MagickImage imageMagick = new MagickImage(stream))
        {
            var newFileName = GenerateNewFileName(fileName);
            var folderName = Path.GetFileNameWithoutExtension(newFileName);
            var directoryPath = Path.Combine(_staticFilesPath, folderName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var originalImagePath = Path.Combine(directoryPath, newFileName);
            await Task.Run(() => imageMagick.Write(originalImagePath));

            var webpFileName = Path.ChangeExtension(newFileName, MagickFormat.WebP.ToString().ToLower());
            var webpImagePath = Path.Combine(directoryPath, webpFileName);
            await Task.Run(() => imageMagick.Write(webpImagePath, MagickFormat.WebP));

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
                    await SaveResizedImage(image, fileName, width, label));

                result = Result.Combine(
                    result,
                    await SaveResizedImage(image, fileName, width, label, MagickFormat.WebP));
            }
        }

        return result;
    }

    private async Task<Result> SaveResizedImage(
        MagickImage imageMagick,
        string originalName,
        int width,
        string nameSuffix,
        MagickFormat? format = null)
    {
        try
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be a non-negative value.");
            }

            MagickGeometry size = new MagickGeometry((uint)width);
            imageMagick.Resize(size);

            var fileName = Path.GetFileNameWithoutExtension(originalName);
            var fileExtension = Path.GetExtension(originalName);

            var folderName = fileName;
            var directoryPath = Path.Combine(_staticFilesPath, folderName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var modifiedFileName = $"{fileName}-{nameSuffix}{(format.HasValue ? $".{format.Value.ToString().ToLower()}" : fileExtension)}";
            var outputImagePath = Path.Combine(directoryPath, modifiedFileName ?? string.Empty);

            await Task.Run(() => imageMagick.Write(outputImagePath, format ?? MagickFormat.Unknown));

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure<string>($"Error saving image: {ex.Message}");
        }
    }


    public async Task<Result> DeleteImages(string[] fileNames)
    {
        var result = Result.Success();

        foreach (var fileName in fileNames)
        {
            var folderName = Path.GetFileNameWithoutExtension(fileName);
            var directoryPath = Path.Combine(_staticFilesPath, folderName);

            if (!Directory.Exists(directoryPath))
            {
                _logger.LogWarning($"Directory not found: {directoryPath}");
                continue;
            }

            try
            {
                await Task.Run(() => Directory.Delete(directoryPath, true));
                _logger.LogInformation($"Successfully deleted directory: {directoryPath}");
            }
            catch (Exception ex)
            {
                result = Result.Combine(result, Result.Failure($"Error deleting directory {directoryPath}: {ex.Message}"));
                _logger.LogError($"Error deleting directory {directoryPath}: {ex.Message}");
            }
        }

        return result;
    }
}
