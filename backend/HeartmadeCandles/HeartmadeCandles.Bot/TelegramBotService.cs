using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot;

public class TelegramBotService : ITelegramBotService
{
    private readonly ILogger<TelegramBotService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ITelegramUserCache _userCache;
    private static readonly string _adminChatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");
    private static readonly string _token = Environment.GetEnvironmentVariable("VAR_TELEGRAM_API_TOKEN");
    private readonly TelegramBotClient _client;

    public TelegramBotService(
        ILogger<TelegramBotService> logger,
        IServiceScopeFactory serviceScopeFactory,
        ITelegramUserCache userCache)
    {
        _client = new TelegramBotClient(_token);
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _userCache = userCache;
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

        if (text.ToLower().Contains(TelegramCommands.InputOrderIdCommand))
        {
            var newUser = user.UpdateState(TelegramUserState.AskingOrderId);

            _userCache.AddOrUpdateUser(newUser);

            await SendOrderIdAsync(botClient, chatId);

            return;
        }
        
        if (text.ToLower().Contains(TelegramCommands.GetOrderInfoCommand))
        {
            await SendOrderInfoAsync(botClient, user.CurrentOrderId, chatId);

            return;
        }

        if (text.ToLower().Contains(TelegramCommands.GetOrderStatusCommand))
        {
            await SendOrderStatusAsync(botClient, user.CurrentOrderId, chatId);

            return;
        }

        if (text.ToLower().Contains(TelegramCommands.GoToCheckoutCommand))
        {
            var newUser = user.UpdateState(TelegramUserState.AskingFullName);

            _userCache.AddOrUpdateUser(newUser);

            await SendPromptFullNameAsync(botClient, chatId);

            return;
        }

        if (user.State == TelegramUserState.AskingOrderId)
        {
            var orderResult = await CheckOrderAsync(text);

            if (orderResult.IsFailure)
            {
                var newUser = user.UpdateState(TelegramUserState.OrderNotExist);

                _userCache.AddOrUpdateUser(newUser);

                await SendOrderProcessingErrorMessage(botClient, chatId);
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

                _userCache.AddOrUpdateUser(newUser);

                await SendInfoAboutCommandsAsync(botClient, chatId);
            }
        }
        else if (user.State == TelegramUserState.AskingFullName )
        {
            var newUser = user.UpdateState(TelegramUserState.AskingPhone);

            _userCache.AddOrUpdateUser(newUser);

            await ForwardFullNameToAdminAsync(botClient, message, user);

            await SendPromptPhoneAsync(botClient, chatId);
        }
        else if (user.State == TelegramUserState.AskingPhone)
        {
            var newUser = user.UpdateState(TelegramUserState.AskingAddress);

            _userCache.AddOrUpdateUser(newUser);

            await ForwardPhoneToAdminAsync(botClient, message, user);

            await SendPromptAddressAsync(botClient, chatId);
        }
        else if (user.State == TelegramUserState.AskingAddress)
        {
            var newUser = user.UpdateState(TelegramUserState.OrderExist);

            _userCache.AddOrUpdateUser(newUser);

            await ForwardAddressToAdminAsync(botClient, message, user);

            await SendConfirmedAsync(botClient, chatId);

            await SendInfoAboutCommandsAsync(botClient, chatId);
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                replyToMessageId: message.MessageId,
                text: $"Вы отправили: {messageText}",
                parseMode: ParseMode.MarkdownV2);
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
        using var scope = _serviceScopeFactory.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        if (orderResult.IsFailure)
        {
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
        using var scope = _serviceScopeFactory.CreateScope();
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
        using var scope = _serviceScopeFactory.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        if (orderResult.IsFailure)
        {
            return Result.Failure(orderResult.Error);
        }
        return Result.Success();
    }
}
