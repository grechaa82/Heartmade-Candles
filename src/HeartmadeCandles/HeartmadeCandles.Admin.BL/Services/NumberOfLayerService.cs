﻿using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services
{
    public class NumberOfLayerService : INumberOfLayerService
    {
        private readonly INumberOfLayerRepository _numberOfLayerServiceRepository;

        public NumberOfLayerService(INumberOfLayerRepository numberOfLayerServiceRepository)
        {
            _numberOfLayerServiceRepository = numberOfLayerServiceRepository;
        }

        public async Task<NumberOfLayer[]> GetAll()
        {
            return await _numberOfLayerServiceRepository.GetAll();
        }

        public async Task<NumberOfLayer> Get(int numberOfLayerId)
        {
            return await _numberOfLayerServiceRepository.Get(numberOfLayerId);
        }

        public async Task Create(NumberOfLayer numberOfLayer)
        {
            await _numberOfLayerServiceRepository.Create(numberOfLayer);
        }

        public async Task Update(NumberOfLayer numberOfLayer)
        {
            await _numberOfLayerServiceRepository.Update(numberOfLayer);
        }

        public async Task Delete(int numberOfLayerId)
        {
            await _numberOfLayerServiceRepository.Delete(numberOfLayerId);
        }
    }
}
