using HeartmadeCandles.Constructor.Core.Models;

namespace HeartmadeCandles.Constructor.Core.Interfaces
{
    public interface IConstructorRepository
    {
        Task<CandleTypeWithCandles[]> GetAll();
    }
}
