using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Admin;

[ApiController]
[Route("api/admin/images")]
[Authorize(Roles = "Admin")]
[EnableRateLimiting("FixedWindowPolicy")]
public class ImageController : Controller
{
    private readonly IImageService _imageService;
    private readonly ILogger<ImageController> _logger;
    private readonly string _staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/Images");

    public ImageController(IImageService imageService, ILogger<ImageController> logger)
    {
        _imageService = imageService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImages(IFormFileCollection formImages)
    {
        if (formImages == null || formImages.Count == 0)
        {
            return BadRequest("No images provided in the request.");
        }

        var imageFiles = formImages
            .Select(file => (file.OpenReadStream(), file.FileName))
            .ToList();

        try
        {
            var result = await _imageService.UploadImages(imageFiles);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to add images, error message: {ex}");
        }
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