using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using HeartmadeCandles.Bot.Core.Models;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Bot.Core;
using HeartmadeCandles.Bot.BL.ReplyMarkups;
using static HeartmadeCandles.Bot.BL.Handlers.MessageHandlers.PhoneAnswerHandler;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class DeliveryTypeAnswerHandler : MessageHandlerBase
{
    public DeliveryTypeAnswerHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        user.State == TelegramUserState.AskingDeliveryType;

    public async override Task Process(Message message, TelegramUser user)
    {
        if (message.Text == null)
        {
            return;
        }

        var text = message.Text.ToLower();

        if (text.Contains("1")
            || text.Contains(DeliveryType.Pochta.ToLower())
            || text.Contains("2")
            || text.Contains(DeliveryType.Sdek.ToLower()))
        {
            await _telegramBotRepository.UpdateTelegramUserState(
                user.ChatId,
                TelegramUserState.AskingAddress);

            var deliveryTypeText = message.Text;

            if (text.Contains("1"))
            {
                deliveryTypeText = DeliveryType.Pochta;
            }
            else if (text.Contains("2"))
            {
                deliveryTypeText = DeliveryType.Sdek;
            }

            await ForwardDeliveryTypeToAdminAsync(
                _botClient, 
                message, 
                user, 
                deliveryTypeText);

            await SendConfirmedAsync(_botClient, message.Chat.Id);

            return;
        }
        else if (text.Contains("3") || text.Contains(DeliveryType.Pickup.ToLower()))
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

            await ForwardDeliveryTypeToAdminAsync(
                _botClient,
                message,
                user,
                DeliveryType.Pickup);

            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: OrderInfoFormatter.EscapeSpecialCharacters(
                    $"""
                    Ваши данные успешно напралвлены администратору.
                    Вы Можете отслеживать статус заказа используя команду {TelegramMessageCommands.GetOrderStatusCommand}
                    
                    Если возникнут сложности он с вами свяжется.
                    """),
                replyMarkup: OrderReplyKeyboardMarkup.GetOrderCommands(),
                parseMode: ParseMode.MarkdownV2);

            return;
        }
        else
        {
            await _telegramBotRepository.UpdateTelegramUserState(
                user.ChatId,
                TelegramUserState.AskingDeliveryType);
            
            return;
        }
    }

    private async Task ForwardDeliveryTypeToAdminAsync(
        ITelegramBotClient botClient,
        Message message,
        TelegramUser user,
        string deliveryType,
        CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: _adminChatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters($"Пользователь {user.UserName} и заказом " +
            $"{user.CurrentOrderId} выбрал тип доставки: {deliveryType}"),
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
                Шаг 4 из 4
                
                Введите свой адрес, куда доставить посылку.
                
                Пример: Санкт-Петербург, Казанская площадь, дом 4, кв. 12
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
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Возникла проблема в заполнение заказа. 
                
                Попробуйте обратиться к администратору или попробовать заполнить данные еще раз {TelegramMessageCommands.GoToCheckoutCommand}
                """),
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: OrderReplyKeyboardMarkup.GetOrderGoToCheckoutCommand(),
            cancellationToken: cancellationToken);
    }
}
