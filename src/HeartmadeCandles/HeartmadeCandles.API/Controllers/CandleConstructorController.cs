using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;
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
        public async Task<IActionResult> GetAll()
        {
            var result = await _candleConstructorService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _candleConstructorService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateOrder(ShoppingCart shoppingCart)
        {
            var result = await _candleConstructorService.CreateOrder(shoppingCart);
            
            return Ok();
        }
    }
}
