using CSharpFunctionalExtensions;
using HeartmadeCandles.Bot.Documents;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeartmadeCandles.Bot.Handlers;

public abstract class CallBackQueryHandlerBase
{
    protected readonly ITelegramBotClient _botClient;
    protected readonly IMongoCollection<TelegramUser> _telegramUserCollection;
    protected readonly IServiceScopeFactory _serviceScopeFactory;
    protected readonly string _adminChatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");

    protected CallBackQueryHandlerBase(
        ITelegramBotClient botClient,
        IMongoDatabase mongoDatabase,
        IServiceScopeFactory serviceScopeFactory)
    {
        _botClient = botClient;
        _telegramUserCollection = mongoDatabase.GetCollection<TelegramUser>(TelegramUser.DocumentName);
        _serviceScopeFactory = serviceScopeFactory;
    }

    public abstract bool ShouldHandleUpdate(CallbackQuery callbackQuery, TelegramUser user);

    public abstract Task Process(CallbackQuery callbackQuery, TelegramUser user);

    public async Task<Maybe<Order.Core.Models.Order>> GetOrdersByStatus(OrderStatus status, int pageSize, int pageIndex = 0)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderMaybe = await orderService.GetOrderByStatus(status, pageSize, pageIndex);

        return orderMaybe.HasValue ? orderMaybe.Value.First() : Maybe.None;
    }

    public async Task<Result<Order.Core.Models.Order>> GetOrderById(string orderId)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        return orderResult;
    }
}
