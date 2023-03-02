using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        #region Decor

        [HttpGet("[action]")]
        public async Task<IActionResult> Decor()
        {
            var decors = await _adminService.GetDecorAsync();
            return Ok(decors);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateDecor(Decor decor)
        {
            await _adminService.CreateDecorAsync(decor);
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateDecor(Decor decor)
        {
            await _adminService.UpdateDecorAsync(decor);
            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteDecor(string id)
        {
            await _adminService.DeleteDecorAsync(id);
            return Ok();
        }

        #endregion
    }
}