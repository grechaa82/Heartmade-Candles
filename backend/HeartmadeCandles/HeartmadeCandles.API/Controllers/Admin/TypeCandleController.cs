using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HeartmadeCandles.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/typeCandles")]
    [Authorize(Roles = "Admin")]
    public class TypeCandleController : Controller
    {
        private readonly ITypeCandleService _typeCandleService;

        public TypeCandleController(ITypeCandleService typeCandleService)
        {
            _typeCandleService = typeCandleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _typeCandleService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _typeCandleService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(TypeCandleRequest typeCandleRequest)
        {
            var result = TypeCandle.Create(typeCandleRequest.Title, typeCandleRequest.Id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _typeCandleService.Create(result.Value);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TypeCandleRequest typeCandleRequest)
        {
            var result = TypeCandle.Create(typeCandleRequest.Title, id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _typeCandleService.Update(result.Value);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _typeCandleService.Delete(id);

            return Ok();
        }
    }
}
