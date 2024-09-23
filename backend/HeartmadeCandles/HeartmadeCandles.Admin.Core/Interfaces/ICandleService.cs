using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ICandleService
{
    Task<(Result<Candle[]>, long)> GetAll(string? typeFilter, PaginationSettings pagination);

    Task<Maybe<CandleDetail>> Get(int candleId);

    Task<Result> Create(Candle candle);

    Task<Result> Update(Candle candle);

    Task<Result> Delete(int candleId);

    Task<Result> UpdateDecor(int candleId, int[] decorIds);

    Task<Result> UpdateLayerColor(int candleId, int[] layerColorIds);

    Task<Result> UpdateNumberOfLayer(int candleId, int[] numberOfLayerIds);

    Task<Result> UpdateSmell(int candleId, int[] smellIds);

    Task<Result> UpdateWick(int candleId, int[] wickIds);
}