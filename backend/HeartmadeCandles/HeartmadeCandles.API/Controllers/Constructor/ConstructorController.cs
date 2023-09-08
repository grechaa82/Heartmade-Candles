using HeartmadeCandles.Constructor.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Constructor
{
    [ApiController]
    [Route("api/constructor")]
    public class ConstructorController : Controller
    {
        private readonly IConstructorService _constructorService;
        private readonly ILogger<ConstructorController> _logger;


        public ConstructorController(IConstructorService constructorService, ILogger<ConstructorController> logger)
        {
            _constructorService = constructorService;
            _logger = logger;
        }

        [HttpGet("candles")]
        public async Task<IActionResult> GetCandles()
        {
            _logger.LogDebug("Request {0} {1}, {2}", 
                nameof(ConstructorController),
                nameof(ConstructorController.GetCandles),
                DateTime.UtcNow);

            var result = await _constructorService.GetCandles();
            
            if (result.IsFailure)
            {
                _logger.LogError("Error request {0} {1}, {2}",
                    nameof(ConstructorController),
                    nameof(ConstructorController.GetCandles),
                    DateTime.UtcNow);

                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("candles/{candleId}")]
        public async Task<IActionResult> GetCandleById(int candleId)
        {
            _logger.LogDebug("Request {0} {1}, candleId: {2}, {3}",
                nameof(ConstructorController),
                nameof(ConstructorController.GetCandleById),
                candleId,
                DateTime.UtcNow);

            var result = await _constructorService.GetCandleDetailById(candleId);

            if (result.IsFailure)
            {
                _logger.LogError("Error request {0} {1}, candleId: {2}, {3}",
                    nameof(ConstructorController),
                    nameof(ConstructorController.GetCandles),
                    candleId,
                    DateTime.UtcNow);

                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
