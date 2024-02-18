using HeartmadeCandles.Bot.BL.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.ReplyMarkups;

internal class AdminKeyboardMarkup
{
    public static ReplyKeyboardMarkup GetAdminCommandsReplyKeyboard()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                $"Работа с заказами {TelegramMessageCommands.GetOrdersByStatusCommand}",
            }
        })
        {
            ResizeKeyboard = true
        };
    }
}
