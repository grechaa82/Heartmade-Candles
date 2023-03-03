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
        public async Task<IActionResult> GetDecor()
        {
            var decors = await _adminService.GetAllAsync<Decor>();
            return Ok(decors);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateDecor(Decor decor)
        {
            await _adminService.CreateAsync(decor);
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateDecor(Decor decor)
        {
            await _adminService.UpdateAsync(decor);
            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteDecor(Decor decor)
        {
            await _adminService.DeleteAsync(decor);
            return Ok();
        }
        #endregion

        #region LayerColor
        [HttpGet("[action]")]
        public async Task<IActionResult> GetLayerColor()
        {
            var decors = await _adminService.GetAllAsync<LayerColor>();
            return Ok(decors);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateLayerColor(LayerColor layerColor)
        {
            await _adminService.CreateAsync(layerColor);
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateLayerColor(LayerColor layerColor)
        {
            await _adminService.UpdateAsync(layerColor);
            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteLayerColor(LayerColor layerColor)
        {
            await _adminService.DeleteAsync(layerColor);
            return Ok();
        }

        #endregion

        #region Smell
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSmell()
        {
            var decors = await _adminService.GetAllAsync<Smell>();
            return Ok(decors);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateSmell(Smell smell)
        {
            await _adminService.CreateAsync(smell);
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateSmell(Smell smell)
        {
            await _adminService.UpdateAsync(smell);
            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteSmell(Smell smell)
        {
            await _adminService.DeleteAsync(smell);
            return Ok();
        }
        #endregion
    }
}