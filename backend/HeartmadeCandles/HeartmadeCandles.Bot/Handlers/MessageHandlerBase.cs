using HeartmadeCandles.Bot.Documents;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeartmadeCandles.Bot.Handlers;

public abstract class MessageHandlerBase
{
    protected readonly ITelegramBotClient _botClient;
    protected readonly IMongoCollection<TelegramUser> _telegramUserCollection;
    protected readonly IServiceScopeFactory _serviceScopeFactory;
    protected readonly string _adminChatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");

    protected MessageHandlerBase(
        ITelegramBotClient botClient,
        IMongoDatabase mongoDatabase,
        IServiceScopeFactory serviceScopeFactory)
    {
        _botClient = botClient;
        _telegramUserCollection = mongoDatabase.GetCollection<TelegramUser>(TelegramUser.DocumentName);
        _serviceScopeFactory = serviceScopeFactory;
    }

    public abstract bool ShouldHandleUpdate(Message message, TelegramUser user);

    public abstract Task Process(Message message, TelegramUser user);
}
