using AutoMapper.Configuration.Conventions;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.Modules.Admin.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CandleController : Controller
    {
        private readonly ICandleService _cadleService;

        public CandleController(ICandleService cadleService)
        {
            _cadleService = cadleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDecor(Decor decor)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _cadleService.GetAll());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _cadleService.Get(id));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Decor decor)
        {
            return Ok();
        }

        [HttpDelete("{id:int/[action]}")]
        public async Task<IActionResult> Delete(int id)
        {
            _cadleService.Delete(id);

            return Ok();
        }
    }
}
