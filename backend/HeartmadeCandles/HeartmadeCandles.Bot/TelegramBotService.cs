using HeartmadeCandles.Bot.HandlerChains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot;

public class TelegramBotService : ITelegramBotService
{
    private readonly ITelegramBotClient _client;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ITelegramUserCache _userCache;
    private readonly HandlerChainBase[] _handlers;
    private readonly ILogger<TelegramBotService> _logger;

    public TelegramBotService(
        ITelegramBotClient client,
        IServiceScopeFactory serviceScopeFactory,
        ITelegramUserCache userCache,
        IEnumerable<HandlerChainBase> handlers,
        ILogger<TelegramBotService> logger)
    {
        _client = client;
        _serviceScopeFactory = serviceScopeFactory;
        _userCache = userCache;
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
            var newUser = new TelegramUser(
                userId: message.From.Id,
                chatId: message.Chat.Id,
                userName: message.From.Username,
                firstName: message.From.FirstName,
                lastName: message.From.LastName,
                state: TelegramUserState.Created,
                role: TelegramUserRole.Buyer);

            _userCache.AddOrUpdateUser(newUser);

            await SendStartMessage(botClient, message, chatId);

            return;
        }

        var user = _userCache.GetByChatId(chatId).Value;

        foreach (var handler in _handlers)
        {
            if (handler.ShouldHandleUpdate(message, user))
            {
                await handler.Process(message, user);
                break;
            }
        }
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
}
