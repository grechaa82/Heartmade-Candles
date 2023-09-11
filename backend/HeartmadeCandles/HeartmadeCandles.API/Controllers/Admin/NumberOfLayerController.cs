using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HeartmadeCandles.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/numberOfLayers")]
    [Authorize(Roles = "Admin")]
    public class NumberOfLayerController : Controller
    {
        private readonly INumberOfLayerService _numberOfLayerService;

        public NumberOfLayerController(INumberOfLayerService numberOfLayerService)
        {
            _numberOfLayerService = numberOfLayerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
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
            var result = NumberOfLayer.Create(numberOfLayerRequest.Number);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _numberOfLayerService.Create(result.Value);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NumberOfLayerRequest numberOfLayerRequest)
        {
            var result = NumberOfLayer.Create(numberOfLayerRequest.Number, id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _numberOfLayerService.Update(result.Value);

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
