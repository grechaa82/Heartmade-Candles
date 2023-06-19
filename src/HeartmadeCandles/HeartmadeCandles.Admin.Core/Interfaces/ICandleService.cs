using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ICandleService
    {
        Task Create(Candle candle);

        Task<Candle[]> GetAll();

        Task<CandleDetail> Get(int id);

        Task Update(Candle candle);

        Task Delete(int id);
        Task<Result> UpdateDecor(int id, int[] decorsIds);
        Task<Result> UpdateLayerColor(int id, int[] layerColorsIds);
        Task<Result> UpdateNumberOfLayer(int id, int[] numberOfLayersIds);
        Task<Result> UpdateSmell(int id, int[] smellsIds);
        Task<Result> UpdateWick(int id, int[] wicksIds);
    }
}
