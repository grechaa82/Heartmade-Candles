using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderService
{
    Task<Result<Models.Order>> GetOrderById(string orderId);

    Task<Result<string>> CreateOrder(User? user, Feedback? feedback, string basketId);
}