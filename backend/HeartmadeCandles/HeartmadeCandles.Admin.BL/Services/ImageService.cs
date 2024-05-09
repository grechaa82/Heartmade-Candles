using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using ImageMagick;

namespace HeartmadeCandles.Admin.BL.Services;

public class ImageService : IImageService
{
    private readonly string _staticFilesPath;

    public ImageService(string staticFilesPath)
    {
        _staticFilesPath = staticFilesPath;
    }

    public async Task<Result<string[]>> UploadImages(List<(Stream, string)> imageFiles)
    {
        var fileNames = new List<string>();

        foreach (var (stream, fileName) in imageFiles)
        {
            var imageResult = await SaveImage(stream, fileName);

            if (imageResult.IsFailure)
            {
                continue;
            }

            fileNames.Add(imageResult.Value);

            await ResizeImage(stream, imageResult.Value);
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

    private async Task<Result> ResizeImage(Stream stream, string fileName)
    {
        using (MagickImage cloneImageMagick = new MagickImage(stream))
        {
            await SaveImageWithResolutionAsync(cloneImageMagick, fileName, 1200, "large");
            await SaveImageWithResolutionWebPAsync(cloneImageMagick, fileName, 1200, "large");
            
            await SaveImageWithResolutionAsync(cloneImageMagick, fileName, 600, "medium");
            await SaveImageWithResolutionWebPAsync(cloneImageMagick, fileName, 600, "medium");

            await SaveImageWithResolutionAsync(cloneImageMagick, fileName, 200, "small");
            await SaveImageWithResolutionWebPAsync(cloneImageMagick, fileName, 200, "small");

            await SaveImageWithResolutionAsync(cloneImageMagick, fileName, 20, "preview");
            await SaveImageWithResolutionWebPAsync(cloneImageMagick, fileName, 20, "preview");

            return Result.Success();
        }
    }

    private async Task SaveImageWithResolutionAsync(MagickImage imageMagick, string originalName, int width, string nameSuffix)
    {
        MagickGeometry size = new MagickGeometry(width);
        imageMagick.Resize(size);

        var fileName = Path.GetFileNameWithoutExtension(originalName);
        var fileExtension = Path.GetExtension(originalName);
        var modifiedFileName = $"{fileName}-{nameSuffix}{fileExtension}";
        var outputImagePath = Path.Combine(_staticFilesPath, modifiedFileName);

        await Task.Run(() => imageMagick.Write(outputImagePath));
    }

    private async Task SaveImageWithResolutionWebPAsync(MagickImage imageMagick, string originalName, int width, string nameSuffix)
    {
        MagickGeometry size = new MagickGeometry(width);
        imageMagick.Resize(size);

        var fileName = Path.GetFileNameWithoutExtension(originalName);
        var modifiedFileName = $"{fileName}-{nameSuffix}.webp";
        var outputImagePath = Path.Combine(_staticFilesPath, modifiedFileName);

        await Task.Run(() => imageMagick.Write(outputImagePath, MagickFormat.WebP));
    }
}
