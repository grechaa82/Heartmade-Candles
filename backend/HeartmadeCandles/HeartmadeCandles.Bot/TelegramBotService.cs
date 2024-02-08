using HeartmadeCandles.Bot.Documents;
using HeartmadeCandles.Bot.HandlerChains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot;

public class TelegramBotService : ITelegramBotService
{
    private readonly ITelegramBotClient _client;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMongoCollection<TelegramUser> _telegramUserCollection;
    private readonly HandlerChainBase[] _handlers;
    private readonly ILogger<TelegramBotService> _logger;

    public TelegramBotService(
        ITelegramBotClient client,
        IServiceScopeFactory serviceScopeFactory,
        IMongoDatabase mongoDatabase,
        IEnumerable<HandlerChainBase> handlers,
        ILogger<TelegramBotService> logger)
    {
        _client = client;
        _serviceScopeFactory = serviceScopeFactory;
        _telegramUserCollection = mongoDatabase.GetCollection<TelegramUser>(TelegramUser.DocumentName);
        _handlers = handlers.ToArray();
        _logger = logger;
    }

    public async Task Update(Update update)
    {
        await HandleStartMessageAsync(_client, update);
    }

    private async Task HandleStartMessageAsync(ITelegramBotClient botClient, Update update)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        var text = message.Text;

        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            return;

        if (text.ToLower().Contains(TelegramCommands.StartCommand))
        {
            await EnsureUserExists(message);

            await SendStartMessage(botClient, message, chatId);

            return;
        }

        var user = await _telegramUserCollection
            .Find(x => x.ChatId == chatId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            await EnsureUserExists(message);
        }

        foreach (var handler in _handlers)
        {
            if (handler.ShouldHandleUpdate(message, user))
            {
                await handler.Process(message, user);
                return;
            }
        }

        return;
    }

    private async Task SendStartMessage(ITelegramBotClient botClient, Message message, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            replyToMessageId: message.MessageId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Добро пожаловать.

                Вам доступны команды: 

                /order - для начала работы с заказом
                """),
            cancellationToken: cancellationToken,
            parseMode: ParseMode.MarkdownV2);
    }

    private async Task EnsureUserExists(Message message)
    {
        var existingUser = await _telegramUserCollection
            .Find(x => x.ChatId == message.Chat.Id)
            .FirstOrDefaultAsync();

        if (existingUser == null)
        {
            var newUser = new TelegramUser(
                userId: message.From.Id,
                chatId: message.Chat.Id,
                userName: message.From.Username,
                firstName: message.From.FirstName,
                lastName: message.From.LastName,
                state: TelegramUserState.Created,
                role: TelegramUserRole.Buyer);

            await _telegramUserCollection.InsertOneAsync(newUser);
        }
    }
}
