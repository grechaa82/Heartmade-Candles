using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HeartmadeCandles.API.Controllers.Admin
{

    [ApiController]
    [Route("api/admin/images")]
    [Authorize(Roles = "Admin")]
    public class ImageController : Controller
    {
        private readonly string _staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/Images");

        [HttpPost]
        public async Task<IActionResult> UploadImages(IFormFileCollection formImages)
        {
            var fileNames = new List<string>();

            foreach (var formImage in formImages)
            {
                var fileName = GenerateFileName(Path.GetExtension(formImage.FileName));

                var addImageResult = await AddImage(formImage, fileName);
                if(addImageResult.IsFailure)
                {
                    return BadRequest(addImageResult.Error);
                }

                fileNames.Add(fileName);
            }

            return Ok(fileNames);

            string GenerateFileName(string extension) => Guid.NewGuid().ToString() + extension;

            async Task<Result> AddImage(IFormFile image, string fileName)
            {
                try
                {
                    var filePath = Path.Combine(_staticFilesPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImages(string[] fileNames)
        {
            try
            {
                foreach (var fileName in fileNames)
                {
                    var imagePath = Path.Combine(_staticFilesPath, fileName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    } 
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}