using HeartmadeCandles.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAsync()
        {
            var userId = Request.Cookies["userId"];

            var shoppingCart = await _shoppingCartService.Get(userId);

            return Ok(shoppingCart);
        }

        [HttpPost("[action]")]
        public IActionResult CreateOrder()
        {
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
