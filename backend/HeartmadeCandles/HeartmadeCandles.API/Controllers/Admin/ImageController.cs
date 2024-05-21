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
                _logger.LogError(
                    "Error: Failed in process {processName}, error message: {errorMessage}",
                    nameof(_imageService.UploadImages),
                    result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to add images, error message: {errorMessage}", ex);
            return BadRequest($"Failed to add images, error message: {ex}");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteImages(string[] fileNames)
    {
        var result = await _imageService.DeleteImages(fileNames);
        
        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_imageService.DeleteImages),
                result.Error);
            return BadRequest($"Failed to delete images, error message: {result.Error}");
        }

        return Ok();
    }
}