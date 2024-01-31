namespace HeartmadeCandles.Order.Bot;

public enum TelegramUserState
{
    None,
    Created,
    AskingOrderId,
    OrderExist,
    OrderNotExist,
    AskingPhone,
    AskingFullName,
    AskingAddress,
}
