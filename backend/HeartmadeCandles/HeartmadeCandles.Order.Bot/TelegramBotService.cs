using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
        _logger.LogError("Telegram Bot Error {ErrorMessage}", exception.Message);
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

            userCache.AddOrUpdateUser(newUser);

            await SendStartMessage(botClient, message, chatId, cancellationToken);

            return;
        }

        var user = userCache.GetByChatId(chatId).Value;

        if (text.ToLower().Contains(TelegramCommands.InputOrderIdCommand))
        {
            var newUser = user.UpdateState(TelegramUserState.AskingOrderId);

            userCache.AddOrUpdateUser(newUser);

            await SendOrderIdAsync(botClient, chatId, cancellationToken);

            return;
        }
        
        if (text.ToLower().Contains(TelegramCommands.GetOrderInfoCommand))
        {
            await SendOrderInfoAsync(botClient, user.CurrentOrderId, chatId, cancellationToken);

            return;
        }

        if (text.ToLower().Contains(TelegramCommands.GetOrderStatusCommand))
        {
            await SendOrderStatusAsync(botClient, user.CurrentOrderId, chatId, cancellationToken);

            return;
        }

        if (text.ToLower().Contains(TelegramCommands.GoToCheckoutCommand))
        {
            var newUser = user.UpdateState(TelegramUserState.AskingFullName);

            userCache.AddOrUpdateUser(newUser);

            await SendPromptFullNameAsync(botClient, chatId, cancellationToken);

            return;
        }

        if (user.State == TelegramUserState.AskingOrderId)
        {
            var orderResult = await CheckOrderAsync(text);

            if (orderResult.IsFailure)
            {
                var newUser = user.UpdateState(TelegramUserState.OrderNotExist);

                userCache.AddOrUpdateUser(newUser);

                await SendOrderProcessingErrorMessage(botClient, chatId, cancellationToken);
            }
            else
            {
                var newUser = new TelegramUser(
                    userId: user.UserId,
                    chatId: user.ChatId,
                    userName: user.UserName,
                    firstName: user.FirstName,
                    lastName: user.LastName,
                    currentOrderId: text,
                    state: TelegramUserState.OrderExist,
                    role: user.Role);

                userCache.AddOrUpdateUser(newUser);

                await SendInfoAboutCommandsAsync(botClient, chatId, cancellationToken);
            }
        }
        else if (user.State == TelegramUserState.AskingFullName )
        {
            var newUser = user.UpdateState(TelegramUserState.AskingPhone);

            userCache.AddOrUpdateUser(newUser);

            await ForwardFullNameToAdminAsync(botClient, message, user, cancellationToken);

            await SendPromptPhoneAsync(botClient, chatId, cancellationToken);
        }
        else if (user.State == TelegramUserState.AskingPhone)
        {
            var newUser = user.UpdateState(TelegramUserState.AskingAddress);

            userCache.AddOrUpdateUser(newUser);

            await ForwardPhoneToAdminAsync(botClient, message, user, cancellationToken);

            await SendPromptAddressAsync(botClient, chatId, cancellationToken);
        }
        else if (user.State == TelegramUserState.AskingAddress)
        {
            var newUser = user.UpdateState(TelegramUserState.OrderExist);
            
            userCache.AddOrUpdateUser(newUser);

            await ForwardAddressToAdminAsync(botClient, message, user, cancellationToken);

            await SendConfirmedAsync(botClient, chatId, cancellationToken);

            await SendInfoAboutCommandsAsync(botClient, chatId, cancellationToken);
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

    private async Task SendInfoAboutCommandsAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Вам доступны команды: 
                    
                {TelegramCommands.GetOrderInfoCommand} - узнать информацию о заказе
                {TelegramCommands.GetOrderStatusCommand} - узнать текущий статус заказа
                {TelegramCommands.GoToCheckoutCommand} - оформить заказ
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task SendOrderIdAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Введите номер заказа: ",
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task SendOrderInfoAsync(ITelegramBotClient botClient, string orderId, long chatId, CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        if (orderResult.IsFailure)
        {
            _logger.LogError("");
            await SendOrderProcessingErrorMessage(botClient, chatId, cancellationToken);
        }

        var message = OrderInfoFormatter.GetOrderInfoInMarkdownV2(orderResult.Value);

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task SendOrderStatusAsync(ITelegramBotClient botClient, string orderId, long chatId, CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        if (orderResult.IsFailure)
        {
            await SendOrderProcessingErrorMessage(botClient, chatId, cancellationToken);
        }

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: orderResult.Value.Status.ToString(),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task SendOrderProcessingErrorMessage(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Возникла проблема с вашим заказом. Мы не смогли его найти. 
                
                Вы можете:
                - Попробовать ввести номер заказа еще раз {TelegramCommands.InputOrderIdCommand}
                - Создать новый заказ на нашем сайте 4fass.ru
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task ForwardFullNameToAdminAsync(ITelegramBotClient botClient, Message message, TelegramUser user, CancellationToken cancellationToken = default)
    {
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
    }

    private async Task ForwardPhoneToAdminAsync(ITelegramBotClient botClient, Message message, TelegramUser user, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: _adminChatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters($"Пользователь {user.UserName} и заказом {user.CurrentOrderId} заполнил свой номер телефона:"),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);

        await botClient.ForwardMessageAsync(
            chatId: _adminChatId,
            fromChatId: user.ChatId,
            messageId: message.MessageId,
            cancellationToken: cancellationToken);
    }

    private async Task ForwardAddressToAdminAsync(ITelegramBotClient botClient, Message message, TelegramUser user, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: _adminChatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters($"Пользователь {user.UserName} и заказом {user.CurrentOrderId} заполнил свой адрес доставки:"),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);

        await botClient.ForwardMessageAsync(
            chatId: _adminChatId,
            fromChatId: user.ChatId,
            messageId: message.MessageId,
            cancellationToken: cancellationToken);
    }

    private async Task SendPromptFullNameAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                """
                Шаг 1 из 3
                
                !!! В данный момент доставка возможно только Почтой России, приносим свои извинения.

                Отправьте одним сообщение вашу Фамилию Имя и Отчество.

                Пример: Константинопольский Константин Владимирович
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task SendPromptPhoneAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                """
                Шаг 2 из 3
                
                Введите ваш номер телефона.
                
                Пример: +7 987 654 32 10
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }
    
    private async Task SendPromptAddressAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                """
                Шаг 3 из 3
                
                Введите свой адрес, куда доставить посылку.
                
                Пример: Санкт-Петербург, Казанская площадь, дом 4, кв. 12
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task SendConfirmedAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        // TODO: Изменить статус заказа на оформленный
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters("Ваши данные успешно напралвлены администратору. Если возникнут сложности он с вами свяжется."),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task<Result> CheckOrderAsync(string orderId)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        if (orderResult.IsFailure)
        {
            return Result.Failure(orderResult.Error);
        }
        return Result.Success();
    }
}
