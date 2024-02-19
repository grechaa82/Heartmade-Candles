using HeartmadeCandles.Bot.Core.Models;
using HeartmadeCandles.Bot.Core;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using HeartmadeCandles.Order.Core.Interfaces;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class GetOrderByIdAnswerHandler : MessageHandlerBase
{
    public GetOrderByIdAnswerHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user)
    {
        return user.Role == TelegramUserRole.Admin
            && user.State == TelegramUserState.AskingOrderById;
    }

    public async override Task Process(Message message, TelegramUser user)
    {
        if (message.Text == null)
        {
            await _botClient.EditMessageTextAsync(
                chatId: message.Chat.Id,
                messageId: message.MessageId,
                text: "Что-то пошло не так",
                parseMode: ParseMode.MarkdownV2);
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(message.Text);

        if (orderResult.IsFailure)
        {
            await _botClient.EditMessageTextAsync(
                chatId: message.Chat.Id,
                messageId: message.MessageId,
                text: "Что-то пошло не так",
                parseMode: ParseMode.MarkdownV2);
        }

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: OrderInfoFormatter.GetOrderInfoInMarkdownV2(orderResult.Value),
            messageThreadId: message.MessageThreadId,
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
