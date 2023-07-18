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
        public async Task<IActionResult> GetCandle()
        {
            return Ok(await _constructorService.GetAll());
        }
    }
}
