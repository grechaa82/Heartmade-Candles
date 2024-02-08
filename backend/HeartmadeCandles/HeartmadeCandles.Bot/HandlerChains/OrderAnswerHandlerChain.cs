using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Bot.Documents;
using MongoDB.Driver;

namespace HeartmadeCandles.Bot.HandlerChains;

public class OrderAnswerHandlerChain : HandlerChainBase
{
    public OrderAnswerHandlerChain(
        ITelegramBotClient botClient,
        IMongoDatabase mongoDatabase,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        user.State == TelegramUserState.AskingOrderId;

    public async override Task Process(Message message, TelegramUser user)
    {
        var orderResult = await CheckOrderAsync(message.Text ?? string.Empty);

        if (orderResult.IsFailure)
        {
            var update = Builders<TelegramUser>.Update
                .Set(x => x.State, TelegramUserState.OrderNotExist);
            
            await _telegramUserCollection.UpdateOneAsync(x => x.ChatId == user.ChatId, update: update);

            await SendOrderProcessingErrorMessage(_botClient, message.Chat.Id);

            return;
        }
        else
        {
            var update = Builders<TelegramUser>.Update
                .Set(x => x.State, TelegramUserState.OrderNotExist)
                .Set(x => x.CurrentOrderId, message.Text);
            
            await _telegramUserCollection.UpdateOneAsync(x => x.ChatId == user.ChatId, update: update);

            await SendInfoAboutCommandsAsync(_botClient, message.Chat.Id);

            return;
        }
    }

    private async Task SendInfoAboutCommandsAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Вам доступны команды: 
                    
                {TelegramCommands.GetOrderInfoCommand} - узнать информацию о заказе
                {TelegramCommands.GetOrderStatusCommand} - узнать текущий статус заказа
                {TelegramCommands.GoToCheckoutCommand} - оформить заказ
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task SendOrderProcessingErrorMessage(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Возникла проблема с вашим заказом. Мы не смогли его найти. 
                
                Вы можете:
                - Попробовать ввести номер заказа еще раз {TelegramCommands.InputOrderIdCommand}
                - Создать новый заказ на нашем сайте 4fass.ru
                """),
            parseMode: ParseMode.MarkdownV2,
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
}
