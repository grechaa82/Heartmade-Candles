namespace HeartmadeCandles.Bot.Core.Models;

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
