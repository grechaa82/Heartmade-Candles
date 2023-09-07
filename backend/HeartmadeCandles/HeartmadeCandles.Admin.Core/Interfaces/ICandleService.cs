using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ICandleService
    {
        Task Create(Candle candle);

        Task<Candle[]> GetAll();

        Task<CandleDetail> Get(int candleId);

        Task Update(Candle candle);

        Task Delete(int candleId);
        Task<Result> UpdateDecor(int candleId, int[] decorIds);
        Task<Result> UpdateLayerColor(int candleId, int[] layerColorIds);
        Task<Result> UpdateNumberOfLayer(int candleId, int[] numberOfLayerIds);
        Task<Result> UpdateSmell(int candleId, int[] smellIds);
        Task<Result> UpdateWick(int candleId, int[] wickIds);
    }
}
