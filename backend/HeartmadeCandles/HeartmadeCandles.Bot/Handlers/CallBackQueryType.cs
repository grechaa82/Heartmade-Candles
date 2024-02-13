namespace HeartmadeCandles.Bot.Handlers;

internal enum CallBackQueryType
{
    GetOrders,

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
