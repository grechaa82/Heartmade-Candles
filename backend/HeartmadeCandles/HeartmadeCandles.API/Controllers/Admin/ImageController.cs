using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin;

[ApiController]
[Route("api/admin/images")]
[Authorize(Roles = "Admin")]
public class ImageController : Controller
{
    private readonly ILogger<ImageController> _logger;
    private readonly string _staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/Images");

    public ImageController(ILogger<ImageController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImages(IFormFileCollection formImages)
    {
        var result = Result.Success();
        var fileNames = new List<string>();

        foreach (var formImage in formImages)
        {
            var fileName = GenerateFileName(Path.GetExtension(formImage.FileName));

            var addImageResult = await AddImage(formImage, fileName);
            if (addImageResult.IsFailure)
            {
                result = Result.Combine(result, Result.Failure($"'{nameof(formImage.FileName)}' was not added"));
            }
            else
            {
                fileNames.Add(fileName);
            }
        }

        if (result.IsFailure)
        {
            return BadRequest($"Failed to add images, error message: {result.Error}");
        }

        return Ok(fileNames);
    }

    private string GenerateFileName(string extension)
    {
        return Guid.NewGuid() + extension;
    }

    private async Task<Result> AddImage(IFormFile image, string fileName)
    {
        try
        {
            var filePath = Path.Combine(_staticFilesPath, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: Failed to add images, error message: {errorMessage}", ex);
            return Result.Failure(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteImages(string[] fileNames)
    {
        var result = Result.Success();

        foreach (var fileName in fileNames)
        {
            var imagePath = Path.Combine(_staticFilesPath, fileName);

            var deleteImageResult = await DeleteImage(imagePath);

            if (deleteImageResult.IsFailure)
            {
                result = Result.Combine(result, Result.Failure($"'{nameof(fileName)}' does not deleted"));
            }
        }

        if (result.IsFailure)
        {
            return BadRequest($"Failed to delete images, error message: {result.Error}");
        }

        return Ok(fileNames);
    }

    private Task<Result> DeleteImage(string imagePath)
    {
        try
        {
            if (!System.IO.File.Exists(imagePath))
            {
                return Task.FromResult(Result.Failure($"{imagePath} does not exist"));
            }

            System.IO.File.Delete(imagePath);

            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: Failed to delete images, error message: {errorMessage}", ex);
            throw;
        }
    }
}