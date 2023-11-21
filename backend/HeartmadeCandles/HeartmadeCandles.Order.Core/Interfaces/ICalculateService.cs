using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface ICalculateService
{
    Result<decimal> CalculatePrice(ConfiguredCandle configuredCandle);
}
