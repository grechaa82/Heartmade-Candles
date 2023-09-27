using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface INumberOfLayerRepository
{
    Task<Maybe<NumberOfLayer[]>> GetAll();
    Task<Maybe<NumberOfLayer>> Get(int numberOfLayerId);
    Task<Maybe<NumberOfLayer[]>> GetByIds(int[] numberOfLayerIds);
    Task<Result> Create(NumberOfLayer numberOfLayer);
    Task<Result> Update(NumberOfLayer numberOfLayer);
    Task<Result> Delete(int numberOfLayerId);
    Task<Result> UpdateCandleNumberOfLayer(int candleId, NumberOfLayer[] numberOfLayers);
}