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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Request.Cookies["userId"];

            var user = await _userService.GetAsync(userId);

            return Ok(user);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateCustomer(
            string name,
            string surname,
            string middleName,
            string phone,
            TypeDelivery typeDelivery)
        {
            var userId = Request.Cookies["userId"];

            await _userService.UpdateCustomerAsync(
                userId,
                name,
                surname,
                middleName,
                phone,
                typeDelivery);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateAddress(
            string country,
            string cities,
            string street,
            string house,
            string flat,
            string index)
        {
            var userId = Request.Cookies["userId"];

            await _userService.UpdateAddressAsync(
                userId,
                country,
                cities,
                street,
                house,
                flat,
                index);

            return Ok();
        }
    }
}