using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces
{
    public interface IOrderCreateHandler
    {
        void OnOrderCreated(Order order);
    }
}