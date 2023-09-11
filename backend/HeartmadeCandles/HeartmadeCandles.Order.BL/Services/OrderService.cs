using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.BL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderNotificationHandler _orderNotificationHandler;

        public OrderService(IOrderRepository orderRepository, IOrderNotificationHandler orderNotificationHandler)
        {
            _orderRepository = orderRepository;
            _orderNotificationHandler = orderNotificationHandler;
        }

        public async Task<Result<OrderItem[]>> Get(OrderItemFilter[] orderItemFilters)
        {
            var result = Result.Success();

            var candleDetailWithQuantityAndPriceResult = await _orderRepository.Get(orderItemFilters);

            if (candleDetailWithQuantityAndPriceResult.IsFailure)
            {
                return candleDetailWithQuantityAndPriceResult;
            }

            result = await ProcessCandleDetails(candleDetailWithQuantityAndPriceResult.Value, orderItemFilters, result);

            if (result.IsFailure)
            {
                return Result.Failure<OrderItem[]>(result.Error);
            }

            return candleDetailWithQuantityAndPriceResult.Value;
        }

        public async Task<Result> CreateOrder(
            string configuredCandlesString, 
            OrderItemFilter[] OrderItemFilters, 
            User user, 
            Feedback feedback)
        {
            var result = Result.Success();

            var orderItemsResult = await _orderRepository.Get(OrderItemFilters);
            if (orderItemsResult.IsFailure)
            {
                return orderItemsResult;
            }

            result = await ProcessCandleDetails(orderItemsResult.Value, OrderItemFilters, result);
            if (result.IsFailure)
            {
                return Result.Failure<OrderItem[]>(result.Error);
            }

            var order = new Core.Models.Order(configuredCandlesString, orderItemsResult.Value, user, feedback);
            var isMessageSend = await _orderNotificationHandler.OnCreateOrder(order);
            if(isMessageSend.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            return Result.Success();
        }

        private async Task<Result> ProcessCandleDetails(
            OrderItem[] candleDetails, 
            OrderItemFilter[] candleDetailIds, 
            Result result)
        {
            for (var i = 0; i < candleDetails.Length; i++)
            {
                var candleDetailWithQuantityAndPrice = candleDetails[i];
                var candleDetailIdsWithQuantity = candleDetailIds[i];

                var resultMatching = await CheckMatching(candleDetailWithQuantityAndPrice, candleDetailIdsWithQuantity);

                if (resultMatching.IsFailure)
                {
                    result = Result.Combine(result, Result.Failure<OrderItem[]>(resultMatching.Error));
                }
            }

            return result;
        }

        private async Task<Result<OrderItem>> CheckMatching(
            OrderItem candleDetailWithQuantityAndPrice,
            OrderItemFilter candleDetailIdsWithQuantity)
        {
            if (candleDetailWithQuantityAndPrice.CandleDetail.Candle.Id != candleDetailIdsWithQuantity.CandleId
                            || (candleDetailWithQuantityAndPrice.CandleDetail.Decor != null
                                && candleDetailWithQuantityAndPrice.CandleDetail.Decor.Id != candleDetailIdsWithQuantity.DecorId)
                            || candleDetailWithQuantityAndPrice.CandleDetail.NumberOfLayer.Id != candleDetailIdsWithQuantity.NumberOfLayerId
                            || candleDetailWithQuantityAndPrice.CandleDetail.NumberOfLayer.Number != candleDetailWithQuantityAndPrice.CandleDetail.LayerColors.Length
                            || candleDetailWithQuantityAndPrice.CandleDetail.Wick.Id != candleDetailIdsWithQuantity.WickId
                            || (candleDetailWithQuantityAndPrice.CandleDetail.Smell != null
                                && candleDetailWithQuantityAndPrice.CandleDetail.Smell.Id != candleDetailIdsWithQuantity.SmellId))
            {
                return Result.Failure<OrderItem>(
                       $"An error occurred in the candle configuration " +
                       $"{candleDetailWithQuantityAndPrice.ToString()}");
            }

            return Result.Success(candleDetailWithQuantityAndPrice);
        }
    }
}
