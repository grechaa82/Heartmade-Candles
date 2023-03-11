using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Request.Cookies["userId"];

            var user = await _userService.GetAsync(userId);

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {
            await _userService.UpdateCustomerAsync(customer);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateAddress(Address address)
        {
            await _userService.UpdateAddressAsync(address);

            return Ok();
        }
    }
}