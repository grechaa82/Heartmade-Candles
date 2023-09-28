using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ISmellRepository
{
    Task<Maybe<Smell[]>> GetAll();
    Task<Maybe<Smell>> Get(int smellId);
    Task<Maybe<Smell[]>> GetByIds(int[] smellIds);
    Task<Result> Create(Smell smell);
    Task<Result> Update(Smell smell);
    Task<Result> Delete(int smellId);
    Task<Result> UpdateCandleSmell(int candleId, Smell[] smells);
}