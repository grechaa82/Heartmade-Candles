﻿using HeartmadeCandles.API.Contracts.Requests;
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

        public CandleController(ICandleService candleService)
        {
            _candleService = candleService;
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

            var candleResult = Candle.Create(
                candleRequest.Title,
                candleRequest.Description,
                candleRequest.Price,
                candleRequest.WeightGrams,
                candleRequest.ImageURL,
                typeCandleResult.Value,
                candleRequest.IsActive);

            if (candleResult.IsFailure)
            {
                return BadRequest(candleResult.Error);
            }

            await _candleService.Create(candleResult.Value);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CandleRequest candleRequest)
        {
            var typeCandleResult = TypeCandle.Create(candleRequest.TypeCandle.Title, candleRequest.TypeCandle.Id);

            if (typeCandleResult.IsFailure)
            {
                return BadRequest(typeCandleResult.Error);
            }

            var candleResult = Candle.Create(
                candleRequest.Title,
                candleRequest.Description,
                candleRequest.Price,
                candleRequest.WeightGrams,
                candleRequest.ImageURL,
                typeCandleResult.Value,
                candleRequest.IsActive,
                id);

            if (candleResult.IsFailure)
            {
                return BadRequest(candleResult.Error);
            }

            await _candleService.Update(candleResult.Value);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _candleService.Delete(id);

            return Ok();
        }

        [HttpPut("{id}/decors")]
        public async Task<IActionResult> UpdateDecor(int id, int[] decorsIds)
        {
            var result = await _candleService.UpdateDecor(id, decorsIds);
            
            if(result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPut("{id}/layerColors")]
        public async Task<IActionResult> UpdateLayerColor(int id, int[] layerColorsIds)
        {
            var result = await _candleService.UpdateLayerColor(id, layerColorsIds);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPut("{id}/numberOfLayers")]
        public async Task<IActionResult> UpdateNumberOfLayer(int id, int[] numberOfLayersIds)
        {
            var result = await _candleService.UpdateNumberOfLayer(id, numberOfLayersIds);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPut("{id}/smells")]
        public async Task<IActionResult> UpdateSmell(int id, int[] smellsIds)
        {
            var result = await _candleService.UpdateSmell(id, smellsIds);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPut("{id}/wicks")]
        public async Task<IActionResult> UpdateWick(int id, int[] wicksIds)
        {
            var result = await _candleService.UpdateWick(id, wicksIds);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> UploadImages(int id, [FromForm]List<IFormFile> imagesToUpload)
        {            
            foreach (var image in imagesToUpload)
            {
                await AddImage(image);
            }

            var result = await _candleService.UpdateImageURL(id, GetImageURL(imagesToUpload));

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPut("{id}/images")]
        public async Task<IActionResult> UpdateImages(int id, string[] imagesURL)
        {            
            var result = await _candleService.UpdateImageURL(id, GetImageURL(imagesURL), false);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpDelete("{id}/images")]
        public async Task<IActionResult> DeleteImages(int id, string[] imageURL)
        {
            foreach (var image in imageURL)
            {
                DeleteImage(image);
            }
         
            var result = await _candleService.DeleteImageURL(id, imageURL);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        private string GetImageURL(List<IFormFile> images)
        {
            string result = ""; 
            
            foreach (var image in images) 
            { 
                result += "http://localhost:5000/StaticFiles/Images/" + image.FileName +  ","; 
            } 
            
            return result.TrimEnd(',');
        }

        public string GetImageURL(string[] imagesURL)
        {
            string result = ""; 
                    
            foreach (var image in imagesURL) 
            { 
                if (!image.StartsWith("http://localhost:5000/StaticFiles/Images/"))
                {
                    result += "http://localhost:5000/StaticFiles/Images/" + image +  ","; 
                }
                else
                {
                    result += image +  ","; 
                }
            } 
                    
            return result.TrimEnd(',');
        }

        private async Task AddImage(IFormFile image)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/Images", image.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
        }

        private void DeleteImage(string imageUrl)
        {
            var imagePath = imageUrl.Replace("http://localhost:5000/", "");

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}
