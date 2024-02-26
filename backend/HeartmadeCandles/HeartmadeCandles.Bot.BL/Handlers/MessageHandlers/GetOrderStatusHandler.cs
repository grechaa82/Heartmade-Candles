using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using HeartmadeCandles.Order.Core.Interfaces;
using Telegram.Bot.Types.Enums;
using HeartmadeCandles.Bot.Core.Models;
using Telegram.Bot.Types.ReplyMarkups;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.BL.Utilities;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class GetOrderStatusHandler : MessageHandlerBase
{
    public GetOrderStatusHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        message.Text?.ToLower().Contains(MessageCommands.GetOrderStatusCommand) ?? false;

    public async override Task Process(Message message, TelegramUser user)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(user.CurrentOrderId ?? string.Empty);

        if (orderResult.IsFailure)
        {
            await SendOrderProcessingErrorMessage(_botClient, message.Chat.Id);

            return;
        }

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: orderResult.Value.Status.ToString(),
            parseMode: ParseMode.MarkdownV2);
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
            cancellationToken: cancellationToken);
    }
}
