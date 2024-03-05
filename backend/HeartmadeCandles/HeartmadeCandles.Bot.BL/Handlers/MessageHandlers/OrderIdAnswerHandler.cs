using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Bot.Core.Models;
using Telegram.Bot.Types.ReplyMarkups;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.BL.Utilities;
using HeartmadeCandles.Bot.BL.Utilities.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class OrderIdAnswerHandler : MessageHandlerBase
{
    public OrderIdAnswerHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        user.State == TelegramUserState.AskingOrderId;

    public async override Task Process(Message message, TelegramUser user)
    {
        var orderResult = await CheckOrderAsync(message.Text ?? string.Empty);

        if (orderResult.IsFailure)
        {
            await _telegramBotRepository.UpdateTelegramUserState(
                message.Chat.Id,
                TelegramUserState.OrderNotExist);

            await SendOrderProcessingErrorMessage(_botClient, message.Chat.Id);

            return;
        }
        else
        {
            var updateOrderStatusResult = await UpdateOrderStatus(message.Text, OrderStatus.Confirmed);

            if (updateOrderStatusResult.IsFailure)
            {
                await SendOrderProcessingErrorMessage(_botClient, message.Chat.Id);

                return;
            }

            await _telegramBotRepository.UpdateTelegramUserState(
                user.ChatId,
                TelegramUserState.OrderExist);

            await _telegramBotRepository.UpdateOrderId(user.ChatId, message.Text);

            await SendInfoAboutCommandsAsync(_botClient, message.Chat.Id);

            return;
        }
    }

    private async Task SendInfoAboutCommandsAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: TelegramMessageFormatter.Format(
                $"""
                Вам доступны команды: 
                    
                {MessageCommands.GetOrderInfoCommand} - показать заказанные свечи
                {MessageCommands.GetOrderStatusCommand} - текущий статус заказа
                {MessageCommands.GoToCheckoutCommand} - оформить заказ и заполинть личную информацию
                """),
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: OrderReplyKeyboardMarkup.GetOrderCommands(),
            cancellationToken: cancellationToken);
    }

    private async Task SendOrderProcessingErrorMessage(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
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
            text: TelegramMessageFormatter.Format(
                $"""
                Возникла проблема с вашим заказом. Мы не смогли его найти. 
                
                Вы можете:
                - Попробовать ввести номер заказа еще раз {MessageCommands.InputOrderIdCommand}
                - Создать новый заказ на нашем сайте 4fass.ru
                """),
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: replyKeyboardMarkup,
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

    private async Task<Result> UpdateOrderStatus(string orderId, OrderStatus status)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderStatus = BotMapping.MapBotOrderStatusToOrderOrderStatus(status);

        var orderResult = await orderService.UpdateOrderStatus(orderId, orderStatus);

        if (orderResult.IsFailure)
        {
            return Result.Failure(orderResult.Error);
        }

        return Result.Success();
    }
}
