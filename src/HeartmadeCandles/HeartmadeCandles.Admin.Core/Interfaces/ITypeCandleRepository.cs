using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces
{
    public interface ITypeCandleRepository
    {
        Task<TypeCandle[]> GetAll();
        Task<TypeCandle> Get(int id);
        Task Create(TypeCandle typeCandle);
        Task Update(TypeCandle typeCandle);
        Task Delete(int id);
    }
}
