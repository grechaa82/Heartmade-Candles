using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeartmadeCandles.Bot.HandlerChains;

public abstract class HandlerChainBase
{
    protected readonly ITelegramBotClient _botClient;
    protected readonly ITelegramUserCache _userCache;
    protected readonly IServiceScopeFactory _serviceScopeFactory;
    protected readonly string _adminChatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");

    protected HandlerChainBase(
        ITelegramBotClient botClient, 
        ITelegramUserCache userCache, 
        IServiceScopeFactory serviceScopeFactory)
    {
        _botClient = botClient;
        _userCache = userCache;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public abstract bool ShouldHandleUpdate(Message message, TelegramUser user);

    public abstract Task Process(Message message, TelegramUser user);
}
