using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HeartmadeCandles.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            var userId = Request.Cookies["userId"];

            var shoppingCart = await _shoppingCartService.Get(userId);

            return Ok(shoppingCart);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateOrder(ShoppingCart shoppingCart)
        {
            var result= await _shoppingCartService.CreateOrder(shoppingCart);

            return Ok();
        }

        [HttpPut("[action]")]
        public IActionResult SetQuantity(string id, int quantity = 1)
        {
            return Ok();
        }

        [HttpPut("[action]")]
        public IActionResult Remove(string id)
        {
            return Ok();
        }
    }
}
