using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Core.Interfaces
{
    public interface IOrderNotificationHandler
    {
        Task<Result> OnCreateOrder(Models.Order order);
    }
}
