using HeartmadeCandles.Bot.Documents;
using HeartmadeCandles.Bot.ReplyMarkups;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using System.Threading;

namespace HeartmadeCandles.Bot.Handlers.CallBackQueryHandlers;

public class GetOrderIdHandler : CallBackQueryHandlerBase
{
    public GetOrderIdHandler(
       ITelegramBotClient botClient,
       IMongoDatabase mongoDatabase,
       IServiceScopeFactory serviceScopeFactory)
       : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(CallbackQuery callbackQuery, TelegramUser user)
    {
        if (user.Role != TelegramUserRole.Admin || callbackQuery.Data == null)
        {
            return false;
        }

        var text = callbackQuery.Data.ToLower();

        return text.Contains(TelegramCallBackQueryCommands.GetOrderIdCommand);
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        var callbackData = callbackQuery.Data!.Split(":");

        await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: callbackData.Last(),
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
