using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using HeartmadeCandles.Bot.Core.Models;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.BL.Utilities;
using HeartmadeCandles.Bot.BL.Utilities.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class AddressAnswerHandler : MessageHandlerBase
{
    public AddressAnswerHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
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

        await _telegramBotRepository.UpdateTelegramUserState(
               user.ChatId,
               TelegramUserState.OrderExist);

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
            text: TelegramMessageFormatter.Format($"Пользователь {user.UserName} и заказом " +
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
            text: TelegramMessageFormatter.Format(
                $"""
                Ваши данные успешно напралвлены администратору.
                Вы Можете отслеживать статус заказа используя команду - {MessageCommands.GetOrderStatusCommand}
                
                Если возникнут сложности он с вами свяжется.
                """),
            replyMarkup: OrderReplyKeyboardMarkup.GetOrderCommands(),
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
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: TelegramMessageFormatter.Format(
                $"""
                Возникла проблема в заполнение заказа. 
                
                Попробуйте обратиться к администратору или попробовать заполнить данные еще раз {MessageCommands.GoToCheckoutCommand}
                """),
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: OrderReplyKeyboardMarkup.GetOrderGoToCheckoutCommand(),
            cancellationToken: cancellationToken);
    }
}
