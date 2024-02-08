using HeartmadeCandles.Bot.Documents;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.HandlerChains;

public class OrderPromptHandlerChain : HandlerChainBase
{
    public OrderPromptHandlerChain(
        ITelegramBotClient botClient,
        IMongoDatabase mongoDatabase,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        message.Text?.ToLower().Contains(TelegramCommands.InputOrderIdCommand) ?? false;

    public async override Task Process(Message message, TelegramUser user)
    {
        var update = Builders<TelegramUser>.Update
            .Set(x => x.State, TelegramUserState.AskingOrderId);
        
        await _telegramUserCollection.UpdateOneAsync(x => x.ChatId == user.ChatId, update: update);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Введите номер заказа: ",
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}