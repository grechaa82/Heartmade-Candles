﻿using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/decors")]
    public class DecorController : Controller
    {
        private readonly IDecorService _decorService;

        public DecorController(IDecorService decorService)
        {
            _decorService = decorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _decorService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _decorService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(DecorRequest decorRequest)
        {
            var imagesResult = decorRequest.Images
                .Select(imageRequest => Image.Create(imageRequest.FileName, imageRequest.AlternativeName))
                .ToArray();

            if (imagesResult.Any(result => result.IsFailure))
            {
                var failedImagesResult = imagesResult.Where(result => result.IsFailure).ToArray();
                var errorMessages = string.Join(", ", failedImagesResult.Select(result => result.Error));
                return BadRequest(errorMessages);
            }

            var images = imagesResult
                .Select(result => result.Value)
                .ToArray();

            var result = Decor.Create(
                decorRequest.Title, 
                decorRequest.Description, 
                decorRequest.Price, 
                images,
                decorRequest.IsActive);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _decorService.Create(result.Value);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DecorRequest decorRequest)
        {
            var imagesResult = decorRequest.Images
                .Select(imageRequest => Image.Create(imageRequest.FileName, imageRequest.AlternativeName))
                .ToArray();

            if (imagesResult.Any(result => result.IsFailure))
            {
                var failedImagesResult = imagesResult.Where(result => result.IsFailure).ToArray();
                var errorMessages = string.Join(", ", failedImagesResult.Select(result => result.Error));
                return BadRequest(errorMessages);
            }

            var images = imagesResult
                .Select(result => result.Value)
                .ToArray();

            var result = Decor.Create(
                decorRequest.Title,
                decorRequest.Description,
                decorRequest.Price,
                images,
                decorRequest.IsActive,
                id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            await _decorService.Update(result.Value);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _decorService.Delete(id);

            return Ok();
        }
    }
}
