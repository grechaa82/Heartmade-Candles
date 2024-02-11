using HeartmadeCandles.Bot.Documents;
using HeartmadeCandles.Bot.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot;

public class TelegramBotUpdateHandler : ITelegramBotUpdateHandler
{
    private readonly ITelegramBotClient _client;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMongoCollection<TelegramUser> _telegramUserCollection;
    private readonly MessageHandlerBase[] _messageHandlers;
    private readonly CallBackQueryHandlerBase[] _callBackQueryHandlers;
    private readonly ILogger<TelegramBotUpdateHandler> _logger;

    public TelegramBotUpdateHandler(
        ITelegramBotClient client,
        IServiceScopeFactory serviceScopeFactory,
        IMongoDatabase mongoDatabase,
        IEnumerable<MessageHandlerBase> messageHandlers,
        IEnumerable<CallBackQueryHandlerBase> callBackQueryHandlers,
        ILogger<TelegramBotUpdateHandler> logger)
    {
        _client = client;
        _serviceScopeFactory = serviceScopeFactory;
        _telegramUserCollection = mongoDatabase.GetCollection<TelegramUser>(TelegramUser.DocumentName);
        _messageHandlers = messageHandlers.ToArray();
        _callBackQueryHandlers = callBackQueryHandlers.ToArray();
        _logger = logger;
    }

    public async Task Update(Update update)
    {
        await HandleStartMessageAsync(_client, update);
    }

    private async Task HandleStartMessageAsync(ITelegramBotClient botClient, Update update)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => HandleMessage(botClient, update.Message),
            UpdateType.CallbackQuery => HandleCallBackQuery(botClient, update.CallbackQuery),
            _ => HandleUnknowMessageAsync(botClient, update)
        };

        try
        {
            await handler;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }

    private async Task HandleMessage(ITelegramBotClient botClient, Message? message, CancellationToken cancellationToken = default)
    {
        if (message == null
            || message.Text == null
            || string.IsNullOrEmpty(message.Text)
            || string.IsNullOrWhiteSpace(message.Text))
        {
            return;
        }

        var chatId = message.Chat.Id;
        var text = message.Text;

        if (text.ToLower().Contains(TelegramMessageCommands.StartCommand))
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

        foreach (var handler in _messageHandlers)
        {
            if (handler.ShouldHandleUpdate(message, user))
            {
                await handler.Process(message, user);
                return;
            }
        }

        return;
    }

    private async Task HandleCallBackQuery(ITelegramBotClient botClient, CallbackQuery? callbackQuery, CancellationToken cancellationToken = default)
    {
        if (callbackQuery == null
            || callbackQuery.Message == null 
            || callbackQuery.Message.Text == null
            || string.IsNullOrEmpty(callbackQuery.Message.Text) 
            || string.IsNullOrWhiteSpace(callbackQuery.Message.Text))
        {
            return;
        }

        var message = callbackQuery.Message;

        var user = await _telegramUserCollection
            .Find(x => x.ChatId == message.Chat.Id)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            await EnsureUserExists(message);
        }

        foreach (var handler in _callBackQueryHandlers)
        {
            if (handler.ShouldHandleUpdate(callbackQuery, user))
            {
                await handler.Process(callbackQuery, user);
                return;
            }
        }

        return;
    }

    private async Task HandleUnknowMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    async Task SendStartMessage(ITelegramBotClient botClient, Message message, long chatId, CancellationToken cancellationToken = default)
    {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                $"Ввести номер заказа {TelegramMessageCommands.InputOrderIdCommand}",
            }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            replyToMessageId: message.MessageId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Добро пожаловать.

                Вам доступны команды: 

                {TelegramMessageCommands.InputOrderIdCommand} - для начала работы с заказом
                """),
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    async Task EnsureUserExists(Message message)
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
