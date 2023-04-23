using HeartmadeCandles.Admin.BL.Services;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/numberOfLayers")]
    public class NumberOfLayerController : Controller
    {
        private readonly INumberOfLayerService _numberOfLayerService;

        public NumberOfLayerController(INumberOfLayerService numberOfLayerService)
        {
            _numberOfLayerService = numberOfLayerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _numberOfLayerService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _numberOfLayerService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(NumberOfLayerRequest numberOfLayerRequest)
        {
            var numberOfLayer = new NumberOfLayer(numberOfLayerRequest.Number);

            await _numberOfLayerService.Create(numberOfLayer);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, NumberOfLayerRequest numberOfLayerRequest)
        {
            var numberOfLayer = new NumberOfLayer(numberOfLayerRequest.Number, id);

            await _numberOfLayerService.Update(numberOfLayer);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _numberOfLayerService.Delete(id);

            return Ok();
        }
    }
}
