using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/wicks")]
    public class WickController : Controller
    {
        private readonly IWickService _wickService;

        public WickController(IWickService wickService)
        {
            _wickService = wickService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _wickService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _wickService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(WickRequest wickRequest)
        {
            var imagesResult = wickRequest.Images
                .Select(imageRequest => Image.Create(imageRequest.FileName, imageRequest.AlternativeName))
                .ToArray();

            if (imagesResult.Any(result => result.IsFailure))
            {
                var failedImagesResult = imagesResult.Where(result => result.IsFailure).ToArray();
                var errorMessages = string.Join(", ", failedImagesResult.Select(result => result.Error));
                return BadRequest(errorMessages);
            }

            var images = imagesResult
                .Select(result => result.Value)
                .ToArray();

            var result = Wick.Create(
                wickRequest.Title,
                wickRequest.Description,
                wickRequest.Price,
                images,
                wickRequest.IsActive);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _wickService.Create(result.Value);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WickRequest wickRequest)
        {
            var imagesResult = wickRequest.Images
                .Select(imageRequest => Image.Create(imageRequest.FileName, imageRequest.AlternativeName))
                .ToArray();

            if (imagesResult.Any(result => result.IsFailure))
            {
                var failedImagesResult = imagesResult.Where(result => result.IsFailure).ToArray();
                var errorMessages = string.Join(", ", failedImagesResult.Select(result => result.Error));
                return BadRequest(errorMessages);
            }

            var images = imagesResult
                .Select(result => result.Value)
                .ToArray();

            var result = Wick.Create(
                wickRequest.Title,
                wickRequest.Description,
                wickRequest.Price,
                images,
                wickRequest.IsActive,
                id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _wickService.Update(result.Value);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _wickService.Delete(id);

            return Ok();
        }
    }
}
