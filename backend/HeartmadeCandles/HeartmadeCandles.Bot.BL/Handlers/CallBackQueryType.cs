namespace HeartmadeCandles.Bot.BL.Handlers;

internal enum CallBackQueryType
{
    GetOrders,
    GetOrderId,
    
    UpdateOrderStatus,
    UpdateToCreated,
    UpdateToConfirmed,
    UpdateToPlaced,
    UpdateToPaid,
    UpdateToInProgress,
    UpdateToPacked,
    UpdateToInDelivery,
    UpdateToCompleted,
    UpdateToCancelled,

    CreatedOrderPrevious,
    CreatedOrderNext,
    CreatedOrderSelect,

    ConfirmedOrderPrevious,
    ConfirmedOrderNext,
    ConfirmedOrderSelect,

    PlacedOrderPrevious,
    PlacedOrderNext,
    PlacedOrderSelect,

    PaidOrderPrevious,
    PaidOrderNext,
    PaidOrderSelect,

    InProgressOrderPrevious,
    InProgressOrderNext,
    InProgressOrderSelect,

    PackedOrderPrevious,
    PackedOrderNext,
    PackedOrderSelect,

    InDeliveryOrderPrevious,
    InDeliveryOrderNext,
    InDeliveryOrderSelect,

    CompletedOrderPrevious,
    CompletedOrderNext,
    CompletedOrderSelect,

    CancelledOrderPrevious,
    CancelledOrderNext,
    CancelledOrderSelect,
}
