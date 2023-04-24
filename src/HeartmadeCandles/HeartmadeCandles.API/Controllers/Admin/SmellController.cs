using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/smells")]
    public class SmellController : Controller
    {
        private readonly ISmellService _smellService;

        public SmellController(ISmellService smellService)
        {
            _smellService = smellService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _smellService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _smellService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(SmellRequest smellRequest)
        {
            var smell = Smell.Create(
                smellRequest.Title,
                smellRequest.Description,
                smellRequest.Price,
                smellRequest.IsActive);

            await _smellService.Create(smell);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, SmellRequest smellRequest)
        {
            var smell = Smell.Create(
                smellRequest.Title,
                smellRequest.Description,
                smellRequest.Price,
                smellRequest.IsActive,
                id);

            await _smellService.Update(smell);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _smellService.Delete(id);

            return Ok();
        }
    }
}
