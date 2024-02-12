using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using HeartmadeCandles.Bot.Documents;
using MongoDB.Driver;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.Handlers.MessageHandlers;

public class AddressAnswerHandler : MessageHandlerBase
{
    public AddressAnswerHandler(
        ITelegramBotClient botClient,
        IMongoDatabase mongoDatabase,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        user.State == TelegramUserState.AskingAddress;

    public async override Task Process(Message message, TelegramUser user)
    {
        var updateOrderStatusResult = await UpdateOrderStatus(user.CurrentOrderId, OrderStatus.Placed);

        if (updateOrderStatusResult.IsFailure)
        {
            await SendOrderProcessingErrorMessage(_botClient, message.Chat.Id);

            return;
        }

        var update = Builders<TelegramUser>.Update
            .Set(x => x.State, TelegramUserState.OrderExist);

        await _telegramUserCollection.UpdateOneAsync(x => x.ChatId == user.ChatId, update: update);

        await ForwardAddressToAdminAsync(_botClient, message, user);

        await SendConfirmedAsync(_botClient, message.Chat.Id);

        return;
    }

    private async Task ForwardAddressToAdminAsync(
        ITelegramBotClient botClient,
        Message message,
        TelegramUser user,
        CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: _adminChatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters($"Пользователь {user.UserName} и заказом " +
            $"{user.CurrentOrderId} заполнил свой адрес доставки:"),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);

        await botClient.ForwardMessageAsync(
            chatId: _adminChatId,
            fromChatId: user.ChatId,
            messageId: message.MessageId,
            cancellationToken: cancellationToken);
    }

    private async Task SendConfirmedAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Ваши данные успешно напралвлены администратору.
                Вы Можете отслеживать статус заказа используя команду {TelegramMessageCommands.GetOrderStatusCommand}
                
                Если возникнут сложности он с вами свяжется.
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task<Result> UpdateOrderStatus(string orderId, OrderStatus status)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.UpdateOrderStatus(orderId, status);

        if (orderResult.IsFailure)
        {
            return Result.Failure(orderResult.Error);
        }

        return Result.Success();
    }

    private async Task SendOrderProcessingErrorMessage(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                $"Оформить заказ {TelegramMessageCommands.GoToCheckoutCommand}",
            }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Возникла проблема в заполнение заказа. 
                
                Попробуйте обратиться к администратору или попробовать заполнить данные еще раз {TelegramMessageCommands.GoToCheckoutCommand}
                """),
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
