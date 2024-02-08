namespace HeartmadeCandles.Bot.Documents;

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
