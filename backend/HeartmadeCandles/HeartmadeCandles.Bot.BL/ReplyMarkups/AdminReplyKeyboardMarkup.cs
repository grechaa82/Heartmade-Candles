using HeartmadeCandles.Bot.BL.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.ReplyMarkups;

internal class AdminReplyKeyboardMarkup
{
    public static ReplyKeyboardMarkup GetAdminCommands()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                $"Работа с заказами {TelegramMessageCommands.GetOrdersByStatusCommand}",
            },
            new KeyboardButton[]
            {
                $"Получить заказа по номеру {TelegramMessageCommands.GetChatIdCommand}",
            }
        })
        {
            ResizeKeyboard = true
        };
    }
}
