using HeartmadeCandles.Bot.BL.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.Utilities.ReplyMarkups;

internal class AdminReplyKeyboardMarkup
{
    public static ReplyKeyboardMarkup GetAdminCommands()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                $"Работа с заказами {MessageCommands.GetOrdersByStatusCommand}",
            },
            new KeyboardButton[]
            {
                $"Получить заказа по номеру {MessageCommands.GetChatIdCommand}",
            }
        })
        {
            ResizeKeyboard = true
        };
    }
}
