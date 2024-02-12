using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using HeartmadeCandles.Order.Core.Interfaces;
using Telegram.Bot.Types.Enums;
using HeartmadeCandles.Bot.Documents;
using MongoDB.Driver;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.Handlers.MessageHandlers;

public class GetOrderInfoHandler : MessageHandlerBase
{
    public GetOrderInfoHandler(
        ITelegramBotClient botClient,
        IMongoDatabase mongoDatabase,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        message.Text?.ToLower().Contains(TelegramMessageCommands.GetOrderInfoCommand) ?? false;

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

        var text = OrderInfoFormatter.GetOrderInfoInMarkdownV2(orderResult.Value);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            parseMode: ParseMode.MarkdownV2);
    }

    private async Task SendOrderProcessingErrorMessage(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                $"Ввести номер заказа {TelegramMessageCommands.InputOrderIdCommand}",
            }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Возникла проблема с вашим заказом. Мы не смогли его найти. 
                
                Вы можете:
                - Попробовать ввести номер заказа еще раз {TelegramMessageCommands.InputOrderIdCommand}
                - Создать новый заказ на нашем сайте 4fass.ru
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }
}
