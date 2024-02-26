using Telegram.Bot.Types;

namespace HeartmadeCandles.Bot.Core.Interfaces;

public interface ITelegramBotUpdateHandler
{
    Task Update(Update update);
}
