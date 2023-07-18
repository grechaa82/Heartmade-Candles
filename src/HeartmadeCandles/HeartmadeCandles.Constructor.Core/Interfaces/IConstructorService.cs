using HeartmadeCandles.Constructor.Core.Models;

namespace HeartmadeCandles.Constructor.Core.Interfaces
{
    public interface IConstructorService
    {
        Task<CandleTypeWithCandles[]> GetAll();
    }
}
