using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public abstract class MessageHandlerBase
{
    protected readonly ITelegramBotClient _botClient;
    protected readonly ITelegramBotRepository _telegramBotRepository;
    protected readonly IServiceScopeFactory _serviceScopeFactory;
    protected readonly string _adminChatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");

    protected MessageHandlerBase(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
    {
        _botClient = botClient;
        _telegramBotRepository = telegramBotRepository;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public abstract bool ShouldHandleUpdate(Message message, TelegramUser user);

    public abstract Task Process(Message message, TelegramUser user);
}
