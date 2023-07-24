using HeartmadeCandles.Constructor.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Constructor
{
    [ApiController]
    [Route("api/constructor")]
    public class ConstructorController : Controller
    {
        private readonly IConstructorService _constructorService;

        public ConstructorController(IConstructorService constructorService)
        {
            _constructorService = constructorService;
        }

        [HttpGet("candles")]
        public async Task<IActionResult> GetCandles()
        {
            return Ok(await _constructorService.GetCandles());
        }

        [HttpGet("candles/{candleId}")]
        public async Task<IActionResult> GetCandleById(int candleId)
        {
            var result = await _constructorService.GetCandleDetailById(candleId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            
            return Ok(result.Value);
        }
    }
}
