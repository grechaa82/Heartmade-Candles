using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Order.Bot;

public class TelegramBotService : IHostedService
{
    private static readonly string _adminChatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");
    private static readonly string _token = Environment.GetEnvironmentVariable("VAR_TELEGRAM_API_TOKEN");
    private readonly TelegramBotClient _client;

    public TelegramBotService(ILogger<TelegramBotClient> logger)
    {
        _client = new TelegramBotClient(_token ?? "");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _client.SendTextMessageAsync(chatId: _adminChatId, text: "Телеграм бот запущен").Wait();

        _client.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: new ReceiverOptions
            {
                // receive all update types
                AllowedUpdates = Array.Empty<UpdateType>()
            },
            cancellationToken: cancellationToken
        );

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken _cancellationToken)
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
            await botClient.SendTextMessageAsync(chatId: chatId,
                replyToMessageId: message.MessageId,
                text: $"Добро пожаловать. Ваш chatId: {chatId}",
                cancellationToken: _cancellationToken,
                parseMode: ParseMode.Markdown);
            
            return;
        }
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken _cancellationToken)
    {
        return Task.CompletedTask;
    }
}
