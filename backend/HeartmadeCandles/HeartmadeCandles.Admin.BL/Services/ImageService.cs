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

            var webpFileName = Path.ChangeExtension(newFileName, MagickFormat.WebP.ToString().ToLower());
            var webpImagePath = Path.Combine(_staticFilesPath, webpFileName);

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
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be a non-negative value.");
            }

            MagickGeometry size = new MagickGeometry((uint)width);
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

    public async Task<Result> DeleteImages(string[] fileNames)
    {
        var result = Result.Success();

        var directory = new DirectoryInfo(_staticFilesPath);

        foreach (var fileName in fileNames)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var files = directory.GetFiles(fileNameWithoutExtension + "*.*");

            if (files.Length == 0)
            {
                _logger.LogWarning($"No files found matching: {fileNameWithoutExtension}");
                continue;
            }

            foreach (FileInfo file in files)
            {
                try
                {
                    await Task.Run(file.Delete);
                }
                catch (Exception ex)
                {
                    result = Result.Combine(
                        result, 
                        Result.Failure($"Error deleting file {file.Name}: {ex.Message}"));

                    _logger.LogError($"Error deleting file {file.Name}: {ex.Message}");
                }
            }
        }

        return result;
    }
}
