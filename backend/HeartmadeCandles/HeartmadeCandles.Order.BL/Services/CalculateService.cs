using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.BL.Services;

public class CalculateService : ICalculateService
{
    public Task<Result<decimal>> CalculatePrice(ConfiguredCandle configuredCandle)
    {
        throw new NotImplementedException();
    }
}
