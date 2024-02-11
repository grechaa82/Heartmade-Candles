using CSharpFunctionalExtensions;
using HeartmadeCandles.Bot.Documents;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.Handlers.CallBackQueryHandlers;

public class GetCreatedOrderHandler : HandlerCallBackQueryBase
{
    public GetCreatedOrderHandler(
       ITelegramBotClient botClient,
       IMongoDatabase mongoDatabase,
       IServiceScopeFactory serviceScopeFactory)
       : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(CallbackQuery callbackQuery, TelegramUser user)
    {
        if (user.Role == TelegramUserRole.Admin && callbackQuery.Data != null)
        {
            return callbackQuery.Data.ToLower().Contains(TelegramCallBackQueryCommands.CallBackQueryCreatedCommand);
        }
        else
        {
            return false;
        }
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        var orderMaybe = await GetOrdersByStatus(TelegramCallBackQueryCommands.GetOrderStatusFromCommand(callbackQuery.Data));

        if (orderMaybe.HasNoValue)
        {
            await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: "Ничего не найдено",
            parseMode: ParseMode.MarkdownV2);
        }

        var text = orderMaybe.Value.Select(OrderInfoFormatter.GetOrderInfoInMarkdownV2).ToArray();

        await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: string.Join(" ", text),
            parseMode: ParseMode.MarkdownV2);

        return;
    }

    private async Task<Maybe<Order.Core.Models.Order[]>> GetOrdersByStatus(OrderStatus status)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderMaybe = await orderService.GetOrderByStatus(status);

        return orderMaybe;
    }
}
