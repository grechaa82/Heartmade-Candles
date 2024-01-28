using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Order.Bot;

public class TelegramBotService : IHostedService
{
    private readonly ILogger<TelegramBotService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private static readonly string _adminChatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");
    private static readonly string _token = Environment.GetEnvironmentVariable("VAR_TELEGRAM_API_TOKEN");
    private readonly TelegramBotClient _client = new TelegramBotClient(_token);
    private List<string> orderIds = new List<string>();
    private bool isAfterOrderCommand = false;

    public TelegramBotService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<TelegramBotService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return RunAsync(cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        _client.SendTextMessageAsync(chatId: _adminChatId, text: "Телеграм бот запущен").Wait();

        var me = await _client.GetMeAsync(cancellationToken: cancellationToken);

        await _client.ReceiveAsync(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            },
            cancellationToken: cancellationToken);
    }

    private Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        return ServeUpdate(botClient, update, cancellationToken);
    }

    private async Task ServeUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var updateLogger = scope.ServiceProvider.GetRequiredService<ILogger<TelegramBotService>>();
        var handler = HandleStartMessageAsync(botClient, update, cancellationToken);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await handler;
        }
        catch (Exception e)
        {
            await HandlePollingErrorAsync(botClient, e, cancellationToken);
        }

        stopwatch.Stop();
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task HandleStartMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        var text = message.Text;

        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            return;

        if (text.ToLower().Contains("/start"))
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                replyToMessageId: message.MessageId,
                text: $"Добро пожаловать Ваш chatId {chatId}",
                cancellationToken: cancellationToken,
                parseMode: ParseMode.Markdown);

            return;
        }

        if (text.ToLower().Contains("/order"))
        {
            isAfterOrderCommand = true;
            await SendOrderIdAsync(chatId, cancellationToken);

            return;
        }

        if (text.ToLower().Contains("/list"))
        {
            var mes = string.Join(" and ", orderIds);
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: string.IsNullOrEmpty(mes) ? "empty" : mes,
                cancellationToken: cancellationToken,
                parseMode: ParseMode.MarkdownV2);

            return;
        }

        if (isAfterOrderCommand)
        {
            orderIds.Add(text);
            isAfterOrderCommand = false;
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "ID заказа успешно сохранен",
                cancellationToken: cancellationToken,
                parseMode: ParseMode.MarkdownV2);
        }
    }

    public async Task<Message> SendOrderIdAsync(long chatId, CancellationToken cancellationToken = default)
    {
        return await _client.SendTextMessageAsync(
            chatId: chatId,
            text: "Введите номер заказа: ",
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }
}
