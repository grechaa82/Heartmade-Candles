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
}
