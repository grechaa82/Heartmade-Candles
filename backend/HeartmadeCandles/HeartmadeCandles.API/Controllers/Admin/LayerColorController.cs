using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HeartmadeCandles.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/layerColors")]
    [Authorize(Roles = "Admin")]
    public class LayerColorController : Controller
    {
        private readonly ILayerColorService _layerColorService;

        public LayerColorController(ILayerColorService layerColorService)
        {
            _layerColorService = layerColorService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _layerColorService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _layerColorService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(LayerColorRequest layerColorRequest)
        {
            var imagesResult = ImageValidator.ValidateImages(layerColorRequest.Images);

            if (imagesResult.IsFailure)
            {
                return BadRequest(imagesResult.Error);
            }

            var result = LayerColor.Create(
                layerColorRequest.Title,
                layerColorRequest.Description,
                layerColorRequest.PricePerGram,
                imagesResult.Value,
                layerColorRequest.IsActive);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _layerColorService.Create(result.Value);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LayerColorRequest layerColorRequest)
        {
            var imagesResult = ImageValidator.ValidateImages(layerColorRequest.Images);

            if (imagesResult.IsFailure)
            {
                return BadRequest(imagesResult.Error);
            }

            var result = LayerColor.Create(
                layerColorRequest.Title,
                layerColorRequest.Description,
                layerColorRequest.PricePerGram,
                imagesResult.Value,
                layerColorRequest.IsActive,
                id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _layerColorService.Update(result.Value);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _layerColorService.Delete(id);

            return Ok();
        }
    }
}
