using HeartmadeCandles.Bot.BL.Handlers;
using HeartmadeCandles.Bot.BL.Handlers.CallBackQueryHandlers;
using HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;
using HeartmadeCandles.Bot.BL.Utilities;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.Services;

public class TelegramBotUpdateHandler : ITelegramBotUpdateHandler
{
    private readonly ITelegramBotClient _client;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly MessageHandlerBase[] _messageHandlers;
    private readonly CallBackQueryHandlerBase[] _callBackQueryHandlers;
    private readonly ILogger<TelegramBotUpdateHandler> _logger;

    public TelegramBotUpdateHandler(
        ITelegramBotClient client,
        IServiceScopeFactory serviceScopeFactory,
        IEnumerable<MessageHandlerBase> messageHandlers,
        IEnumerable<CallBackQueryHandlerBase> callBackQueryHandlers,
        ILogger<TelegramBotUpdateHandler> logger)
    {
        _client = client;
        _serviceScopeFactory = serviceScopeFactory;
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

        if (text.ToLower().Contains(MessageCommands.StartCommand))
        {
            await EnsureUserExists(message);

            await SendStartMessage(botClient, message, chatId);

            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var telegramBotRepository = scope.ServiceProvider.GetRequiredService<ITelegramBotRepository>();

        var userMaybe = await telegramBotRepository.GetTelegramUserByChatId(chatId);

        if (!userMaybe.HasValue)
        {
            await EnsureUserExists(message);
        }

        foreach (var handler in _messageHandlers)
        {
            if (handler.ShouldHandleUpdate(message, userMaybe.Value))
            {
                await handler.Process(message, userMaybe.Value);
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

        using var scope = _serviceScopeFactory.CreateScope();

        var telegramBotRepository = scope.ServiceProvider.GetRequiredService<ITelegramBotRepository>();

        var userMaybe = await telegramBotRepository.GetTelegramUserByChatId(message.Chat.Id);

        if (!userMaybe.HasValue)
        {
            await EnsureUserExists(message);
        }

        foreach (var handler in _callBackQueryHandlers)
        {
            if (handler.ShouldHandleUpdate(callbackQuery, userMaybe.Value))
            {
                await handler.Process(callbackQuery, userMaybe.Value);
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
                $"Ввести номер заказа {MessageCommands.InputOrderIdCommand}",
            }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            replyToMessageId: message.MessageId,
            text: TelegramMessageFormatter.Format(
                $"""
                Добро пожаловать.

                Вам доступны команды: 

                {MessageCommands.InputOrderIdCommand} - для начала работы с заказом
                """),
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    async Task EnsureUserExists(Message message)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var telegramBotRepository = scope.ServiceProvider.GetRequiredService<ITelegramBotRepository>();

        var userMaybe = await telegramBotRepository.GetTelegramUserByChatId(message.Chat.Id);

        if (!userMaybe.HasValue)
        {
            var newUser = new TelegramUser(
                userId: message.From.Id,
                chatId: message.Chat.Id,
                userName: message.From.Username,
                firstName: message.From.FirstName,
                lastName: message.From.LastName,
                state: TelegramUserState.Created,
                role: TelegramUserRole.Buyer);

            await telegramBotRepository.CreateTelegramUser(newUser);
        }
    }
}
