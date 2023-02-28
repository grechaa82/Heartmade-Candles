using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface ICandleConstructorService
    {
        Task<List<Decor>> GetAllAsync();
    }
}
