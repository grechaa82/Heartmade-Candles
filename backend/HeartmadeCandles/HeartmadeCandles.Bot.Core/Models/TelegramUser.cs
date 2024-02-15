using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HeartmadeCandles.Bot.Core.Models;

public class TelegramUser
{
    public const string DocumentName = "telegramUser";

    public TelegramUser(
        long userId,
        long chatId,
        string? userName,
        string firstName = "",
        string lastName = "",
        string currentOrderId = "",
        TelegramUserState state = TelegramUserState.None,
        TelegramUserRole role = TelegramUserRole.Buyer,
        string? id = null)
    {
        UserId = userId;
        ChatId = chatId;
        UserName = userName ?? $"{firstName}{(string.IsNullOrEmpty(lastName) ? string.Empty : " " + lastName)}";
        FirstName = firstName;
        LastName = lastName;
        CurrentOrderId = currentOrderId;
        State = state;
        Role = role;
        Id = id ?? ObjectId.GenerateNewId().ToString();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; }

    public long UserId { get; init; }

    public long ChatId { get; init; }

    public string? UserName { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string? CurrentOrderId { get; init; }

    public TelegramUserState State { get; init; }

    public TelegramUserRole Role { get; init; }
}
