using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services;

public class NumberOfLayerService : INumberOfLayerService
{
    private readonly INumberOfLayerRepository _numberOfLayerServiceRepository;

    public NumberOfLayerService(INumberOfLayerRepository numberOfLayerServiceRepository)
    {
        _numberOfLayerServiceRepository = numberOfLayerServiceRepository;
    }

    public async Task<Maybe<NumberOfLayer[]>> GetAll()
    {
        return await _numberOfLayerServiceRepository.GetAll();
    }

    public async Task<Maybe<NumberOfLayer>> Get(int numberOfLayerId)
    {
        return await _numberOfLayerServiceRepository.Get(numberOfLayerId);
    }

    public async Task<Result> Create(NumberOfLayer numberOfLayer)
    {
        return await _numberOfLayerServiceRepository.Create(numberOfLayer);
    }

    public async Task<Result> Update(NumberOfLayer numberOfLayer)
    {
        return await _numberOfLayerServiceRepository.Update(numberOfLayer);
    }

    public async Task<Result> Delete(int numberOfLayerId)
    {
        return await _numberOfLayerServiceRepository.Delete(numberOfLayerId);
    }
}