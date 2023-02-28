using HeartmadeCandles.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandleConstructorController : Controller
    {
        private readonly ICandleConstructorService _candleConstructorService;

        public CandleConstructorController(ICandleConstructorService candleConstructorService)
        {
            _candleConstructorService = candleConstructorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var candles = await _candleConstructorService.GetAllAsync();
            return Ok(candles);
        }
    }
}
