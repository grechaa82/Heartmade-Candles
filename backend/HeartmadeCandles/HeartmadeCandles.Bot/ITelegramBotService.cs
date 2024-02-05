using Telegram.Bot.Types;

namespace HeartmadeCandles.Bot;

public interface ITelegramBotService
{
    Task Update(Update update);
}
