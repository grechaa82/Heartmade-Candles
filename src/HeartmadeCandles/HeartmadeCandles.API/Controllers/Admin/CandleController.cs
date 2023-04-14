using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.Modules.Admin.Core.Interfaces;
using HeartmadeCandles.Modules.Admin.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandleController : Controller
    {
        private readonly ICandleService _cadleService;

        public CandleController(ICandleService cadleService)
        {
            _cadleService = cadleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _cadleService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _cadleService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CandleRequest candleRequest)
        {
            if (!Enum.IsDefined(typeof(TypeCandle), candleRequest.TypeCandle))
            {
                throw new InvalidOperationException();
            }

            var candle = new Candle(
                candleRequest.Title, 
                candleRequest.Description, 
                candleRequest.ImageURL,
                candleRequest.WeightGrams,
                candleRequest.IsActive,
                (TypeCandle)Enum.Parse(typeof(TypeCandle), candleRequest.TypeCandle));

            _cadleService.Create(candle);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, CandleRequest candleRequest)
        {
            var candle = new Candle(
                candleRequest.Title,
                candleRequest.Description,
                candleRequest.ImageURL,
                candleRequest.WeightGrams,
                candleRequest.IsActive,
                (TypeCandle)Enum.Parse(typeof(TypeCandle), candleRequest.TypeCandle),
                id);

            _cadleService.Update(candle);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cadleService.Delete(id);

            return Ok();
        }
    }
}
