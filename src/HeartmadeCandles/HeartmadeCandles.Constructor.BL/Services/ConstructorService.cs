﻿using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Constructor.Core.Models;
using System.Diagnostics;

namespace HeartmadeCandles.Constructor.BL.Services
{
    public class ConstructorService : IConstructorService
    {
        private readonly IConstructorRepository _constructorRepository;

        public ConstructorService(IConstructorRepository constructorRepository)
        {
            _constructorRepository = constructorRepository;
        }

        public async Task<CandleTypeWithCandles[]> GetCandles()
        {
            return await _constructorRepository.GetCandles();
        }

        public async Task<Result<CandleDetail>> GetCandleDetailById(int candleId)
        {
            return await _constructorRepository.GetCandleById(candleId)
                .ToResult($"Candle with id '{candleId}' not found");
        }
    }
}
