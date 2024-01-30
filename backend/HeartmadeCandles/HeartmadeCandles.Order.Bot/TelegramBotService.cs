using HeartmadeCandles.Order.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Order.Bot;

public class TelegramBotService : IHostedService
{
    private readonly ILogger<TelegramBotService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private static readonly string _adminChatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");
    private static readonly string _token = Environment.GetEnvironmentVariable("VAR_TELEGRAM_API_TOKEN");
    private readonly TelegramBotClient _client = new TelegramBotClient(_token);

    public TelegramBotService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<TelegramBotService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return RunAsync(cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        _client.SendTextMessageAsync(chatId: _adminChatId, text: "Телеграм бот запущен").Wait();

        var me = await _client.GetMeAsync(cancellationToken: cancellationToken);

        await _client.ReceiveAsync(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            },
            cancellationToken: cancellationToken);
    }

    private Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        return ServeUpdate(botClient, update, cancellationToken);
    }

    private async Task ServeUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var updateLogger = scope.ServiceProvider.GetRequiredService<ILogger<TelegramBotService>>();
        var handler = HandleStartMessageAsync(botClient, update, cancellationToken);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await handler;
        }
        catch (Exception e)
        {
            await HandlePollingErrorAsync(botClient, e, cancellationToken);
        }

        stopwatch.Stop();
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task HandleStartMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var userCache = scope.ServiceProvider.GetRequiredService<ITelegramUserCache>();


        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        var text = message.Text;

        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            return;

        if (text.ToLower().Contains("/start"))
        {
            var newUser = new TelegramUser(
                userId: message.From.Id,
                chatId: message.Chat.Id,
                userName: message.From.Username,
                firstName: message.From.FirstName,
                lastName: message.From.LastName,
                state: TelegramUserState.None,
                role: TelegramUserRole.Buyer);

            userCache.AddOrUpdateUser(newUser);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                replyToMessageId: message.MessageId,
                text: OrderInfoFormatter.EscapeSpecialCharacters("Добро пожаловать. Ваш chatId {chatId}"),
                cancellationToken: cancellationToken,
                parseMode: ParseMode.MarkdownV2);

            return;
        }

        var user = userCache.GetByChatId(chatId).Value;

        if (text.ToLower().Contains("/order"))
        {
            var newUser = new TelegramUser(
                userId: user.UserId,
                chatId: user.ChatId,
                userName: user.UserName,
                firstName: user.FirstName,
                lastName: user.LastName,
                state: TelegramUserState.AskingOrderId,
                role: user.Role);

            userCache.AddOrUpdateUser(newUser);

            await SendOrderIdAsync(chatId, cancellationToken);

            return;
        }
        
        if (text.ToLower().Contains("/getorderinfo"))
        {
            await SendOrderInfoAsync(user.CurrentOrderId, chatId, cancellationToken);

            return;
        }

        if (text.ToLower().Contains("/getorderstatus"))
        {
            await SendOrderStatusAsync(user.CurrentOrderId, chatId, cancellationToken);

            return;
        }

        if (text.ToLower().Contains("/fillfeedback"))
        {
            var newUser = new TelegramUser(
                userId: user.UserId,
                chatId: user.ChatId,
                userName: user.UserName,
                firstName: user.FirstName,
                lastName: user.LastName,
                currentOrderId: user.CurrentOrderId,
                state: TelegramUserState.AskingFeedback,
                role: user.Role);

            userCache.AddOrUpdateUser(newUser);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: OrderInfoFormatter.EscapeSpecialCharacters(
                    """
                    Отправьте одним сообщение вашу Фамилию Имя и Отчество.

                    Пример: Константинопольский Константин Владимирович
                    """),
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);

            return;
        }

        if (text.ToLower().Contains("/fiiladdress"))
        {
            var newUser = new TelegramUser(
                userId: user.UserId,
                chatId: user.ChatId,
                userName: user.UserName,
                firstName: user.FirstName,
                lastName: user.LastName,
                currentOrderId: user.CurrentOrderId,
                state: TelegramUserState.AskingAddress,
                role: user.Role);

            userCache.AddOrUpdateUser(newUser);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: OrderInfoFormatter.EscapeSpecialCharacters(
                    """
                    В данный момент доставка возможно только Почтой России, приносим свои извинения.
                    Введите свой адрес, куда доставить посылку.
                    Пример: Казанская площадь, 2, Санкт-Петербург
                    """),
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);

            return;
        }

        if (user.State == TelegramUserState.AskingOrderId)
        {
            var newUser = new TelegramUser(
                userId: user.UserId,
                chatId: user.ChatId,
                userName: user.UserName,
                firstName: user.FirstName,
                lastName: user.LastName,
                currentOrderId: text,
                state: TelegramUserState.None,
                role: user.Role);

            userCache.AddOrUpdateUser(newUser);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: OrderInfoFormatter.EscapeSpecialCharacters(
                    """
                    Вам доступны команды: 
                    
                    /getorderinfo - узнать информацию о заказе
                    /getorderstatus - узнать текущий статус заказа
                    /fillfeedback - ввести ФИО
                    /fiiladdress - ввести адрес доставки
                    """),
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);
        }
        else if (user.State == TelegramUserState.AskingFeedback)
        {
            var newUser = new TelegramUser(
                userId: user.UserId,
                chatId: user.ChatId,
                userName: user.UserName,
                firstName: user.FirstName,
                lastName: user.LastName,
                currentOrderId: user.CurrentOrderId,
                state: TelegramUserState.None,
                role: user.Role);

            userCache.AddOrUpdateUser(newUser);

            await botClient.SendTextMessageAsync(
                chatId: _adminChatId,
                text: OrderInfoFormatter.EscapeSpecialCharacters($"Пользователь {user.UserName} и заказом {user.CurrentOrderId} заполнил свое ФИО:"),
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);

            await botClient.ForwardMessageAsync(
                chatId: _adminChatId,
                fromChatId: user.ChatId,
                messageId: message.MessageId,
                cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: OrderInfoFormatter.EscapeSpecialCharacters(
                    """
                    Вам доступны команды:
                    
                    /getorderinfo - узнать информацию о заказе
                    /getorderstatus - узнать текущий статус заказа
                    /fillfeedback - ввести ФИО
                    /fiiladdress - ввести адрес доставки
                    """),
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);
        }
        else if (user.State == TelegramUserState.AskingAddress)
        {
            var newUser = new TelegramUser(
                userId: user.UserId,
                chatId: user.ChatId,
                userName: user.UserName,
                firstName: user.FirstName,
                lastName: user.LastName,
                currentOrderId: user.CurrentOrderId,
                state: TelegramUserState.None,
                role: user.Role);

            userCache.AddOrUpdateUser(newUser);

            await botClient.SendTextMessageAsync(
                chatId: _adminChatId,
                text: OrderInfoFormatter.EscapeSpecialCharacters($"Пользователь {user.UserName} и заказом {user.CurrentOrderId} заполнил свое адрес доставки:"),
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);

            await botClient.ForwardMessageAsync(
                chatId: _adminChatId,
                fromChatId: user.ChatId,
                messageId: message.MessageId,
                cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: OrderInfoFormatter.EscapeSpecialCharacters(
                    """
                    Вам доступны команды: 
                    
                    /getorderinfo - узнать информацию о заказе
                    /getorderstatus - узнать текущий статус заказа
                    /fillfeedback - ввести ФИО
                    /fiiladdress - ввести адрес доставки
                    """),
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                replyToMessageId: message.MessageId,
                text: $"Вы отправили: {messageText}",
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);
        }
    }

    public async Task<Message> SendOrderIdAsync(long chatId, CancellationToken cancellationToken = default)
    {
        return await _client.SendTextMessageAsync(
            chatId: chatId,
            text: "Введите номер заказа: ",
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    public async Task<Message> SendOrderInfoAsync(string orderId, long chatId, CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        if (orderResult.IsFailure)
        {
            return await _client.SendTextMessageAsync(
                chatId: chatId,
                text: "Возникла проблема с вашим заказом",
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);
        }

        var message = OrderInfoFormatter.GetOrderInfoInMarkdownV2(orderResult.Value);

        return await _client.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    public async Task<Message> SendOrderStatusAsync(string orderId, long chatId, CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        if (orderResult.IsFailure)
        {
            return await _client.SendTextMessageAsync(
                chatId: chatId,
                text: "Возникла проблема с вашим заказом",
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken);
        }

        return await _client.SendTextMessageAsync(
            chatId: chatId,
            text: orderResult.Value.Status.ToString(),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }
}
