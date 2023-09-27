using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface INumberOfLayerService
{
    Task<Maybe<NumberOfLayer[]>> GetAll();
    Task<Maybe<NumberOfLayer>> Get(int numberOfLayerId);
    Task<Result> Create(NumberOfLayer numberOfLayer);
    Task<Result> Update(NumberOfLayer numberOfLayer);
    Task<Result> Delete(int numberOfLayerId);
}