using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/candles")]
    public class CandleController : Controller
    {
        private readonly ICandleService _candleService;
        private readonly IDecorService _decorService;
        private readonly ILayerColorService _layerColorService;
        private readonly INumberOfLayerService _numberOfLayerService;
        private readonly ISmellService _smellService;
        private readonly ITypeCandleService _typeCandleService;
        private readonly IWickService _wickService;


        public CandleController(
            ICandleService candleService, 
            IDecorService decorService, 
            ILayerColorService layerColorService, 
            INumberOfLayerService numberOfLayerService, 
            ISmellService smellService,
            ITypeCandleService typeCandleService,
            IWickService wickService)
        {
            _candleService = candleService;
            _decorService = decorService;
            _layerColorService = layerColorService;
            _numberOfLayerService = numberOfLayerService;
            _smellService = smellService;
            _typeCandleService = typeCandleService;
            _wickService = wickService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _candleService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _candleService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CandleRequest candleRequest)
        {
            var typeCandleResult = TypeCandle.Create(candleRequest.TypeCandle.Title, candleRequest.TypeCandle.Id);

            if (typeCandleResult.IsFailure)
            {
                return BadRequest(typeCandleResult.Error);
            }

            var result = Candle.Create(
                candleRequest.Title,
                candleRequest.Description,
                candleRequest.Price,
                candleRequest.WeightGrams,
                candleRequest.ImageURL,
                typeCandleResult.Value,
                candleRequest.IsActive);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _candleService.Create(result.Value);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCandleDetailsRequest updateRequest)
        {
            var decors = new List<Decor>();
            foreach (var decorId in updateRequest.DecorsIds)
            {
                var decor = await _decorService.Get(decorId);
                
                if(decor == null)
                {
                    return BadRequest($"Decor with Id '${decorId}' does not exist");
                }
                    
                decors.Add(decor);
            }

            var layerColors = new List<LayerColor>();
            foreach (var layerColorId in updateRequest.LayerColorsIds)
            {
                var layerColor = await _layerColorService.Get(layerColorId);

                if (layerColor == null)
                {
                    return BadRequest($"LayerColor with Id '${layerColorId}' does not exist");
                }

                layerColors.Add(layerColor);
            }

            var numberOfLayers = new List<NumberOfLayer>();
            foreach (var numberOfLayerId in updateRequest.NumberOfLayersIds)
            {
                var numberOfLayer = await _numberOfLayerService.Get(numberOfLayerId);

                if (numberOfLayer == null)
                {
                    return BadRequest($"NumberOfLayer with Id '${numberOfLayerId}' does not exist");
                }

                numberOfLayers.Add(numberOfLayer);
            }

            var smells = new List<Smell>();
            foreach (var smellId in updateRequest.SmellsIds)
            {
                var smell = await _smellService.Get(smellId);

                if (smell == null)
                {
                    return BadRequest($"Smell with Id '${smellId}' does not exist");
                }

                smells.Add(smell);
            }

            var wicks = new List<Wick>();
            foreach (var wickId in updateRequest.WicksIds)
            {
                var wick = await _wickService.Get(wickId);

                if (wick == null)
                {
                    return BadRequest($"Wick with Id '${wickId}' does not exist");
                }

                wicks.Add(wick);
            }

            var typeCandle = await _typeCandleService.Get(updateRequest.CandleRequest.TypeCandle.Id);

            if (typeCandle == null)
            {
                return BadRequest($"TypeCandle with Id '${updateRequest.CandleRequest.TypeCandle.Id}' does not exist");
            }

            var candle = Candle.Create(
                updateRequest.CandleRequest.Title,
                updateRequest.CandleRequest.Description,
                updateRequest.CandleRequest.Price,
                updateRequest.CandleRequest.WeightGrams,
                updateRequest.CandleRequest.ImageURL,
                typeCandle,
                updateRequest.CandleRequest.IsActive,
                id);

            if (candle.IsFailure)
            {
                return BadRequest(candle.Error);
            }

            var candleDetail = CandleDetail.Create(candle.Value, decors, layerColors, numberOfLayers, smells, wicks);

            if (candleDetail.IsFailure)
            {
                return BadRequest(candleDetail.Error);
            }

            await _candleService.Update(candleDetail.Value);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _candleService.Delete(id);

            return Ok();
        }
    }
}
