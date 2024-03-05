using HeartmadeCandles.Bot.Core.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HeartmadeCandles.Bot.DAL.Documents;

public class TelegramUserDocument
{
    public const string DocumentName = "telegramUser";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public long UserId { get; set; }

    public long ChatId { get; set; }

    public string? UserName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CurrentOrderId { get; set; }

    public TelegramUserState State { get; set; }

    public TelegramUserRole Role { get; set; }
}
