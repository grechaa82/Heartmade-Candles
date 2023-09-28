using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services;

public class TypeCandleService : ITypeCandleService
{
    private readonly ITypeCandleRepository _typeCandleRepository;

    public TypeCandleService(ITypeCandleRepository typeCandleRepository)
    {
        _typeCandleRepository = typeCandleRepository;
    }

    public async Task<Maybe<TypeCandle[]>> GetAll()
    {
        return await _typeCandleRepository.GetAll();
    }

    public async Task<Maybe<TypeCandle>> Get(int typeCandleId)
    {
        return await _typeCandleRepository.Get(typeCandleId);
    }

    public async Task<Result> Create(TypeCandle typeCandle)
    {
        return await _typeCandleRepository.Create(typeCandle);
    }

    public async Task<Result> Update(TypeCandle typeCandle)
    {
        return await _typeCandleRepository.Update(typeCandle);
    }

    public async Task<Result> Delete(int typeCandleId)
    {
        return await _typeCandleRepository.Delete(typeCandleId);
    }
}