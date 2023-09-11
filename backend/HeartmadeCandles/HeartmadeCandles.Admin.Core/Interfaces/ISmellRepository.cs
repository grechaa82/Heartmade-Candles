﻿using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ISmellRepository
    {
        Task<Smell[]> GetAll();
        Task<Smell> Get(int smellId);
        Task<Smell[]> GetByIds(int[] smellIds);
        Task Create(Smell smell);
        Task Update(Smell smell);
        Task Delete(int smellId);
        Task UpdateCandleSmell(int candleId, Smell[] smells);
    }
}