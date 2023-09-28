using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services;

public class WickService : IWickService
{
    private readonly IWickRepository _wickRepository;

    public WickService(IWickRepository wickRepository)
    {
        _wickRepository = wickRepository;
    }

    public async Task<Maybe<Wick[]>> GetAll()
    {
        return await _wickRepository.GetAll();
    }

    public async Task<Maybe<Wick>> Get(int wickId)
    {
        return await _wickRepository.Get(wickId);
    }

    public async Task<Result> Create(Wick wick)
    {
        return await _wickRepository.Create(wick);
    }

    public async Task<Result> Update(Wick wick)
    {
        return await _wickRepository.Update(wick);
    }

    public async Task<Result> Delete(int wickId)
    {
        return await _wickRepository.Delete(wickId);
    }
}