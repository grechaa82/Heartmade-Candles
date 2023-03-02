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

        #region Smell

        [HttpGet("[action]")]
        public async Task<IActionResult> Smell()
        {
            var smells = await _adminService.GetSmellAsync();
            return Ok(smells);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateSmell(Smell smell)
        {
            await _adminService.CreateSmellAsync(smell);
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateSmell(Smell smell)
        {
            await _adminService.UpdateSmellAsync(smell);
            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteSmell(string id)
        {
            await _adminService.DeleteSmellAsync(id);
            return Ok();
        }

        #endregion
    }
}